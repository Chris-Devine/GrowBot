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
    [Route("api/growsetting")]
    public class GrowSettingController : ApiController
    {
        private readonly IGrowSettingsRepository _growSettingsRepository;
        private readonly IGrowSettingFactory _userGrowFactory;
        private readonly IUserHelper _userHelper;



    public GrowSettingController(IGrowSettingsRepository growSettingsRepository, IGrowSettingFactory userGrowFactory,
            IUserHelper userHelper)
        {
            _growSettingsRepository = growSettingsRepository;
            _userGrowFactory = userGrowFactory;
            _userHelper = userHelper;
        }

        // GET: api/growsetting
        [Route("api/growsetting/{publicGrows:bool?}")]
        [ResponseType(typeof (List<GrowSettingDto>))]
        public IHttpActionResult GetUserGrow(bool publicGrows = false)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                List<GrowSetting> userGrows = _growSettingsRepository.GetGrowSettings(publicGrows, currentUserGuid);

                if (userGrows == null || userGrows.Count == 0)
                {
                    return NotFound();
                }

                return Ok(userGrows.Select(item => _userGrowFactory.GetGrowSetting(item)).ToList());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/growsetting/5
        [Route("api/growsetting/{growSettingId}")]
        [ResponseType(typeof (GrowSettingDto))]
        public IHttpActionResult GetUserGrow(int growSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                GrowSetting growSetting = _growSettingsRepository.GetGrowSetting(growSettingId, currentUserGuid);

                if (growSetting == null)
                {
                    return NotFound();
                }

                return Ok(_userGrowFactory.GetGrowSetting(growSetting));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // PUT: api/growsetting/5
        [Route("api/growsetting/{growSettingId}")]
        [ResponseType(typeof (void))]
        public IHttpActionResult PutUserGrow(int growSettingId, GrowSettingDto userGrow)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Guid currentUserGuid = _userHelper.GetUserGuid();

                GrowSetting originalEntity = _growSettingsRepository.GetGrowSetting(growSettingId, currentUserGuid);

                if (originalEntity == null)
                {
                    return NotFound();
                }

                GrowSetting modifiedGrowSetting = _userGrowFactory.PutGrowSetting(originalEntity, userGrow);

                RepositoryActionResult<GrowSetting> result = _growSettingsRepository.PutGrowSetting(growSettingId,
                    modifiedGrowSetting, currentUserGuid);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Updated:
                        return Ok(_userGrowFactory.GetGrowSetting(modifiedGrowSetting));
                    case RepositoryActionStatus.Error:
                        return InternalServerError();
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                    case RepositoryActionStatus.NothingModified:
                        return Ok(_userGrowFactory.GetGrowSetting(modifiedGrowSetting));
                    default:
                        return BadRequest();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/growsetting
        [ResponseType(typeof (GrowSettingDto))]
        public IHttpActionResult PostUserGrow(GrowSettingDto userGrow)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Guid currentUserGuid = _userHelper.GetUserGuid();

                GrowSetting entity = _userGrowFactory.PostGrowSetting(userGrow, currentUserGuid);

                RepositoryActionResult<GrowSetting> result = _growSettingsRepository.PostGrowSetting(entity);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    GrowSettingDto newUserGrow = _userGrowFactory.GetGrowSetting(result.Entity);
                    userGrow.GrowSettingId = entity.GrowSettingId;
                    return Created(Request.RequestUri
                                   + "/" + result.Entity.GrowSettingId, newUserGrow);
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // DELETE: api/growsetting/5
        [Route("api/growsetting/{growSettingId}")]
        [ResponseType(typeof (GrowSettingDto))]
        public IHttpActionResult DeleteUserGrow(int growSettingId)
        {
            try
            {
                Guid currentUserGuid = _userHelper.GetUserGuid();

                RepositoryActionResult<GrowSetting> result = _growSettingsRepository.DeleteGrowSetting(growSettingId,
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