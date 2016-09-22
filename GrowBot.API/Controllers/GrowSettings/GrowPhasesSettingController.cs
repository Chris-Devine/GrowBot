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
    [Route("api/growsetting/{growSettingId}/growphasesetting")]
    public class GrowPhaseSettingsController : ApiController
    {
        private readonly IGrowSettingsRepository _growSettingsRepository;
        private readonly IGrowPhaseSettingFactory _GrowPhaseSettingFactory;
        private readonly IUserHelper _userHelper;

        public GrowPhaseSettingsController(IGrowSettingsRepository growSettingsRepository,
            IGrowPhaseSettingFactory GrowPhaseSettingFactory, IUserHelper userHelper)
        {
            _growSettingsRepository = growSettingsRepository;
            _GrowPhaseSettingFactory = GrowPhaseSettingFactory;
            _userHelper = userHelper;
        }

        // GET: api/GrowPhaseSettings
        [Route("api/growsetting/{growSettingId}/growphasesetting")]
        [ResponseType(typeof (List<GrowPhaseSettingDto>))]
        public IHttpActionResult GetGrowPhaseSetting(int growSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                List<GrowPhaseSetting> userGrowsPhase = _growSettingsRepository.GetGrowPhaseSettings(growSettingId,
                    currentUserGuid);

                if (userGrowsPhase == null || userGrowsPhase.Count == 0)
                {
                    return NotFound();
                }

                return Ok(userGrowsPhase.Select(item => _GrowPhaseSettingFactory.GetGrowPhaseSetting(item)).ToList());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/GrowPhaseSettings/5
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}")]
        [ResponseType(typeof (GrowPhaseSettingDto))]
        public IHttpActionResult GetGrowPhaseSetting(int growSettingId, int growPhaseSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                GrowPhaseSetting GrowPhaseSetting = _growSettingsRepository.GetGrowPhaseSetting(growSettingId, growPhaseSettingId,
                    currentUserGuid);

                if (GrowPhaseSetting == null)
                {
                    return NotFound();
                }

                return Ok(_GrowPhaseSettingFactory.GetGrowPhaseSetting(GrowPhaseSetting));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // PUT: api/GrowPhaseSettings/5
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}")]
        [ResponseType(typeof (void))]
        public IHttpActionResult PutGrowPhaseSetting(int growSettingId, int growPhaseSettingId, GrowPhaseSettingDto growPhaseSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                GrowPhaseSetting originalEntity = _growSettingsRepository.GetGrowPhaseSetting(growSettingId, growPhaseSettingId,
                    currentUserGuid);

                if (originalEntity == null)
                {
                    return NotFound();
                }

                GrowPhaseSetting modifiedGrowPhaseSetting = _GrowPhaseSettingFactory.PutGrowPhaseSetting(originalEntity,
                    growPhaseSetting);

                RepositoryActionResult<GrowPhaseSetting> result = _growSettingsRepository.PutGrowPhaseSetting(
                    growPhaseSettingId, modifiedGrowPhaseSetting,
                    currentUserGuid);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Updated:
                        return Ok(_GrowPhaseSettingFactory.GetGrowPhaseSetting(modifiedGrowPhaseSetting));
                    case RepositoryActionStatus.Error:
                        return InternalServerError();
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                    case RepositoryActionStatus.NothingModified:
                        return Ok(_GrowPhaseSettingFactory.GetGrowPhaseSetting(modifiedGrowPhaseSetting));
                    default:
                        return BadRequest();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/GrowPhaseSettings
        [Route("api/growsetting/{growSettingId}/growphasesetting/")]
        [ResponseType(typeof (GrowPhaseSettingDto))]
        public IHttpActionResult PostGrowPhaseSetting(GrowPhaseSettingDto growPhaseSetting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                GrowPhaseSetting entity = _GrowPhaseSettingFactory.PostGrowPhaseSetting(growPhaseSetting);

                RepositoryActionResult<GrowPhaseSetting> result = _growSettingsRepository.PostGrowPhaseSetting(entity,
                    currentUserGuid);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    GrowPhaseSettingDto newGrowPhaseSetting = _GrowPhaseSettingFactory.GetGrowPhaseSetting(result.Entity);
                    growPhaseSetting.GrowPhaseSettingId = entity.GrowPhaseSettingId;
                    //TODO: this is wrong return address, find out correct one and replace
                    return Created(Request.RequestUri
                                   + "/" + result.Entity.GrowSettingId, newGrowPhaseSetting);
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // DELETE: api/GrowPhaseSettings/5
        [Route("api/growsetting/{growSettingId}/growphasesetting/{growPhaseSettingId}")]
        [ResponseType(typeof (GrowPhaseSettingDto))]
        public IHttpActionResult DeleteGrowPhaseSetting(int growSettingId, int growPhaseSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                RepositoryActionResult<GrowPhaseSetting> result = _growSettingsRepository.DeleteGrowPhaseSetting(growSettingId,
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