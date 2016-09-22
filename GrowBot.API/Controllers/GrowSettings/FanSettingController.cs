using System;
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
    [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/fansetting")]
    public class FanSettingController : ApiController
    {
        private readonly IFanSettingFactory _fanSettingFactory;
        private readonly IGrowSettingsRepository _growSettingsRepository;
        private readonly IUserHelper _userHelper;

        public FanSettingController(IGrowSettingsRepository growSettingsRepository, IFanSettingFactory fanSettingFactory,
            IUserHelper userHelper)
        {
            _growSettingsRepository = growSettingsRepository;
            _fanSettingFactory = fanSettingFactory;
            _userHelper = userHelper;
        }

        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/fansetting/{fanSettingsId}")]
        // GET: api/usergrow/1/GrowPhaseSetting/2/fansetting/14
        [ResponseType(typeof (FanSettingDto))]
        public IHttpActionResult GetFanSetting(int growSettingId, int growPhaseSettingId, int fanSettingsId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                FanSetting fanSetting = _growSettingsRepository.GetFanSetting(growSettingId, growPhaseSettingId, fanSettingsId,
                    currentUserGuid);

                if (fanSetting == null)
                {
                    return NotFound();
                }

                return Ok(_fanSettingFactory.GetFanSetting(fanSetting));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}/fansetting/{fanSettingsId}")]
        // PUT: api/usergrow/1/GrowPhaseSetting/2/fansetting/14
        [ResponseType(typeof (void))]
        public IHttpActionResult PutFanSetting(int growSettingId, int growPhaseSettingId, int fanSettingsId,
            FanSettingDto GrowPhaseSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                FanSetting originalEntity = _growSettingsRepository.GetFanSetting(growSettingId, growPhaseSettingId,
                    fanSettingsId,
                    currentUserGuid);

                if (originalEntity == null)
                {
                    return NotFound();
                }

                FanSetting modifiedGrowPhaseSetting = _fanSettingFactory.PutFanSetting(originalEntity, GrowPhaseSetting);

                RepositoryActionResult<FanSetting> result = _growSettingsRepository.PutFanSetting(fanSettingsId,
                    modifiedGrowPhaseSetting, currentUserGuid);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Updated:
                        return Ok(_fanSettingFactory.GetFanSetting(modifiedGrowPhaseSetting));
                    case RepositoryActionStatus.Error:
                        return InternalServerError();
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                    case RepositoryActionStatus.NothingModified:
                        return Ok(_fanSettingFactory.GetFanSetting(modifiedGrowPhaseSetting));
                    default:
                        return BadRequest();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/usergrow/1/growphasesetting/2/fansetting
        [ResponseType(typeof (FanSettingDto))]
        public IHttpActionResult PostFanSetting(FanSettingDto fanSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                FanSetting entity = _fanSettingFactory.PostFanSetting(fanSetting);

                RepositoryActionResult<FanSetting> result = _growSettingsRepository.PostFanSetting(entity,
                    currentUserGuid);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    FanSettingDto newGrowPhaseSetting = _fanSettingFactory.GetFanSetting(result.Entity);
                    fanSetting.GrowPhaseSettingId = entity.GrowPhaseSettingId;
                    //TODO: this is wrong return address, find out correct one and replace
                    return Created(Request.RequestUri
                                   + "/" + result.Entity.FanSettingId, newGrowPhaseSetting);
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // DELETE: api/usergrow/1/growphasesetting/2/fansetting
        [ResponseType(typeof (FanSettingDto))]
        public IHttpActionResult DeleteFanSetting(int growSettingId, int growPhaseSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                RepositoryActionResult<FanSetting> result = _growSettingsRepository.DeleteFanSetting(growSettingId,
                    growPhaseSettingId, currentUserGuid);

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