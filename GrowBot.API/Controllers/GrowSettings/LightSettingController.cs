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
    [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/lightsettings")]
    public class LightSettingController : ApiController
    {
        private readonly IGrowSettingsRepository _growSettingsRepository;
        private readonly ILightSettingFactory _lightSettingFactory;
        private readonly IUserHelper _userHelper;

        public LightSettingController(IGrowSettingsRepository growSettingsRepository,
            ILightSettingFactory lightSettingFactory, IUserHelper userHelper)
        {
            _growSettingsRepository = growSettingsRepository;
            _lightSettingFactory = lightSettingFactory;
            _userHelper = userHelper;
        }

        // GET: api/usergrow/1/GrowPhaseSetting/2/lightsettings
        [ResponseType(typeof (List<LightSettingDto>))]
        public IHttpActionResult GetLightSettings(int growSettingId, int growPhaseSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                List<LightSetting> lightSettings = _growSettingsRepository.GetLightSettings(growSettingId, growPhaseSettingId,
                    currentUserGuid);

                if (lightSettings == null || lightSettings.Count == 0)
                {
                    return NotFound();
                }

                return Ok(lightSettings.Select(item => _lightSettingFactory.GetLightSetting(item)).ToList());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/usergrow/1/GrowPhaseSetting/2/lightsettings/32
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/lightsettings/{lightSettingsId}")]
        [ResponseType(typeof (LightSettingDto))]
        public IHttpActionResult GetLightSetting(int growSettingId, int growPhaseSettingId, int lightSettingsId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                LightSetting lightSetting = _growSettingsRepository.GetLightSetting(growSettingId, growPhaseSettingId,
                    lightSettingsId,
                    currentUserGuid);

                if (lightSetting == null)
                {
                    return NotFound();
                }

                return Ok(_lightSettingFactory.GetLightSetting(lightSetting));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // PUT: api/usergrow/1/GrowPhaseSetting/2/lightsettings/32
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/lightsettings/{lightSettingsId}")]
        [ResponseType(typeof (void))]
        public IHttpActionResult PutLightSetting(int growSettingId, int growPhaseSettingId, int lightSettingsId,
            LightSettingDto lightSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                LightSetting originalEntity = _growSettingsRepository.GetLightSetting(growSettingId, growPhaseSettingId,
                    lightSettingsId, currentUserGuid);

                if (originalEntity == null)
                {
                    return NotFound();
                }

                LightSetting modifiedGrowPhaseSetting = _lightSettingFactory.PutLightSetting(originalEntity, lightSetting);

                RepositoryActionResult<LightSetting> result = _growSettingsRepository.PutLightSetting(growSettingId,
                    growPhaseSettingId, lightSettingsId, modifiedGrowPhaseSetting,
                    currentUserGuid);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Updated:
                        return Ok(_lightSettingFactory.GetLightSetting(modifiedGrowPhaseSetting));
                    case RepositoryActionStatus.Error:
                        return InternalServerError();
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                    case RepositoryActionStatus.NothingModified:
                        return Ok(_lightSettingFactory.GetLightSetting(modifiedGrowPhaseSetting));
                    default:
                        return BadRequest();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/usergrow/1/GrowPhaseSetting/2/lightsettings
        [ResponseType(typeof (LightSettingDto))]
        public IHttpActionResult PostLightSetting(int growSettingId, int growPhaseSettingId, LightSettingDto lightSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                LightSetting entity = _lightSettingFactory.PostLightSetting(lightSetting);

                RepositoryActionResult<LightSetting> result = _growSettingsRepository.PostLightSetting(entity,
                    currentUserGuid);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    LightSettingDto newLightSetting = _lightSettingFactory.GetLightSetting(result.Entity);
                    lightSetting.LightSettingId = entity.LightSettingId;
                    //TODO: this is wrong return address, find out correct one and replace
                    return Created(Request.RequestUri
                                   + "/" + result.Entity.LightSettingId, newLightSetting);
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/lightsettings/{lightSettingsId}")]
        [ResponseType(typeof (LightSettingDto))]
        public IHttpActionResult DeleteLightSetting(int growSettingId, int growPhaseSettingId, int lightSettingsId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                RepositoryActionResult<LightSetting> result = _growSettingsRepository.DeleteLightSetting(growSettingId,
                    growPhaseSettingId, lightSettingsId,
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