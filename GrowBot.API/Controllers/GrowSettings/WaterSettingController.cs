using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.API.Helpers.HelpersInterfaces;
using GrowBot.API.Repository;
using GrowBot.API.Repository.Repositories.GrowSettings;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Controllers.GrowSettings
{
    [Authorize]
    [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/watersettings")]
    public class WaterSettingController : ApiController
    {
        private readonly IGrowSettingsRepository _growSettingsRepository;
        private readonly IUserHelper _userHelper;
        private readonly IWaterSettingFactory _waterSettingFactory;

        public WaterSettingController(IGrowSettingsRepository growSettingsRepository,
            IWaterSettingFactory waterSettingFactory, IUserHelper userHelper)
        {
            _growSettingsRepository = growSettingsRepository;
            _waterSettingFactory = waterSettingFactory;
            _userHelper = userHelper;
        }

        // GET: api/usergrow/1/growphasesetting/2/watersettings
        [ResponseType(typeof (List<WaterSettingDto>))]
        public IHttpActionResult GetWaterSettings(int growSettingId, int growPhaseSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                List<WaterSetting> waterSettings = _growSettingsRepository.GetWaterSettings(growSettingId, growPhaseSettingId,
                    currentUserGuid);

                if (waterSettings == null | waterSettings.Count == 0)
                {
                    return NotFound();
                }

                return Ok(waterSettings.Select(item => _waterSettingFactory.GetWaterSetting(item)).ToList());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/usergrow/1/GrowPhaseSetting/2/watersettings/32
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/watersettings/{waterSettingId}")]
        [ResponseType(typeof (WaterSettingDto))]
        public IHttpActionResult GetWaterSetting(int growSettingId, int growPhaseSettingId, int waterSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                WaterSetting waterSetting = _growSettingsRepository.GetWaterSetting(growSettingId, growPhaseSettingId,
                    waterSettingId,
                    currentUserGuid);

                if (waterSetting == null)
                {
                    return NotFound();
                }

                return Ok(_waterSettingFactory.GetWaterSetting(waterSetting));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // PUT: api/usergrow/1/GrowPhaseSetting/2/watersettings/32
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/watersettings/{waterSettingId}")]
        [ResponseType(typeof (void))]
        public IHttpActionResult PutWaterSetting(int growSettingId, int growPhaseSettingId, int waterSettingId,
            WaterSettingDto waterSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                WaterSetting originalEntity = _growSettingsRepository.GetWaterSetting(growSettingId, growPhaseSettingId,
                    waterSettingId, currentUserGuid);

                if (originalEntity == null)
                {
                    return NotFound();
                }

                WaterSetting modifiedWaterSetting = _waterSettingFactory.PutWaterSetting(originalEntity, waterSetting);

                RepositoryActionResult<WaterSetting> result = _growSettingsRepository.PutWaterSetting(growSettingId,
                    growPhaseSettingId, waterSettingId, modifiedWaterSetting,
                    currentUserGuid);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Updated:
                        return Ok(_waterSettingFactory.GetWaterSetting(modifiedWaterSetting));
                    case RepositoryActionStatus.Error:
                        return InternalServerError();
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                    case RepositoryActionStatus.NothingModified:
                        return Ok(_waterSettingFactory.GetWaterSetting(modifiedWaterSetting));
                    default:
                        return BadRequest();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/usergrow/1/GrowPhaseSetting/2/watersettings
        [ResponseType(typeof (WaterSettingDto))]
        public IHttpActionResult PostWaterSetting(int growSettingId, int GrowPhaseSettingId, WaterSettingDto waterSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                WaterSetting entity = _waterSettingFactory.PostWaterSetting(waterSetting);

                RepositoryActionResult<WaterSetting> result = _growSettingsRepository.PostWaterSetting(entity,
                    currentUserGuid);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    WaterSettingDto newWaterSetting = _waterSettingFactory.GetWaterSetting(result.Entity);
                    waterSetting.WaterSettingId = entity.WaterSettingId;
                    //TODO: this is wrong return address, find out correct one and replace
                    return Created(Request.RequestUri
                                   + "/" + result.Entity.WaterSettingId, newWaterSetting);
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/watersettings/{waterSettingId}")]
        [ResponseType(typeof (LightSettingDto))]
        public IHttpActionResult DeleteWaterSetting(int growSettingId, int growPhaseSettingId, int waterSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                RepositoryActionResult<WaterSetting> result = _growSettingsRepository.DeleteWaterSetting(growSettingId,
                    growPhaseSettingId, waterSettingId,
                    currentUserGuid);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Deleted:
                        return StatusCode(HttpStatusCode.NoContent);
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}