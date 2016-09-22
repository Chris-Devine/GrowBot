using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Management.Instrumentation;
using GrowBot.API.DbContext;
using GrowBot.API.Entities.GrowSettings;

namespace GrowBot.API.Repository.Repositories.GrowSettings
{
    public class GrowSettingsRepository : IGrowSettingsRepository
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public GrowSettingsRepository()
        {
            
        }

        public GrowSettingsRepository(ApplicationDbContext dbContext = null)
        {
            if (dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
            }
            else
            {
                _dbContext = dbContext;
            }
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        #region UserGrow

        public List<GrowSetting> GetGrowSettings(bool publicGrows, Guid currentUserGuid)
        {
            if (publicGrows)
            {
                return
                    _dbContext.GrowSetting.Where(ug => ug.UserGuid == currentUserGuid)
                        .ToList();
            }
            return _dbContext.GrowSetting.Where(ug => ug.UserGuid == currentUserGuid).ToList();
        }

        public GrowSetting GetGrowSetting(int growSettingId, Guid currentUserGuid)
        {
            return
                _dbContext.GrowSetting.FirstOrDefault(ug =>  ug.GrowSettingId == growSettingId && (ug.UserGuid == currentUserGuid));
        }

        public RepositoryActionResult<GrowSetting> PutGrowSetting(int growSettingId, GrowSetting modifiedGrowSetting,
            Guid currentUserGuid)
        {
            if (modifiedGrowSetting.UserGuid != currentUserGuid)
            {
                _dbContext.Entry(modifiedGrowSetting).State = EntityState.Unchanged;
                return new RepositoryActionResult<GrowSetting>(modifiedGrowSetting, RepositoryActionStatus.NotFound);
            }

            //_dbContext.Entry(modifiedUserGrow).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserGrowExists(growSettingId))
                {
                    return new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.Error, ex);
                }
                throw;
            }
            return new RepositoryActionResult<GrowSetting>(modifiedGrowSetting, RepositoryActionStatus.Updated);
        }

        public RepositoryActionResult<GrowSetting> PostGrowSetting(GrowSetting entity)
        {
            try
            {
                _dbContext.GrowSetting.Add(entity);
                int result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<GrowSetting>(entity, RepositoryActionStatus.Created);
                }
                return new RepositoryActionResult<GrowSetting>(entity, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<GrowSetting> DeleteGrowSetting(int growSettingId, Guid currentUserGuid)
        {
            try
            {
                GrowSetting growSetting =
                    _dbContext.GrowSetting.FirstOrDefault(
                        ug => ug.GrowSettingId == growSettingId && ug.UserGuid == currentUserGuid);
                if (growSetting != null)
                {
                    _dbContext.GrowSetting.Remove(growSetting);

                    _dbContext.SaveChanges();
                    return new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.Deleted);
                }
                return new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.NotFound);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        private bool UserGrowExists(int id)
        {
            return _dbContext.GrowSetting.Count(e => e.GrowSettingId == id) > 0;
        }

        #endregion

        #region GrowPhaseSetting

        public List<GrowPhaseSetting> GetGrowPhaseSettings(int growSettingId, Guid currentUserGuid)
        {
            return
                _dbContext.GrowPhaseSetting.Where(
                    ug => ug.GrowSetting.UserGuid == currentUserGuid && ug.GrowSettingId == growSettingId)
                    .ToList();
        }

        public GrowPhaseSetting GetGrowPhaseSetting(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid)
        {
            return
                _dbContext.GrowPhaseSetting.FirstOrDefault(
                    ug => ug.GrowSetting.UserGuid == currentUserGuid && ug.GrowPhaseSettingId == GrowPhaseSettingId);
        }

        public RepositoryActionResult<GrowPhaseSetting> PutGrowPhaseSetting(int GrowPhaseSettingId,
            GrowPhaseSetting modifiedUserGrow, Guid currentUserGuid)
        {
            if (modifiedUserGrow.GrowSetting.UserGuid != currentUserGuid)
            {
                return new RepositoryActionResult<GrowPhaseSetting>(modifiedUserGrow, RepositoryActionStatus.NotFound);
            }

            //_dbContext.Entry(modifiedUserGrow).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!GrowPhaseSettingExists(GrowPhaseSettingId))
                {
                    return new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.Error, ex);
                }
                throw;
            }
            return new RepositoryActionResult<GrowPhaseSetting>(modifiedUserGrow, RepositoryActionStatus.Updated);
        }

        public RepositoryActionResult<GrowPhaseSetting> PostGrowPhaseSetting(GrowPhaseSetting entity, Guid currentUserGuid)
        {
            try
            {
                GrowSetting growSetting = GetGrowSetting(entity.GrowSettingId, currentUserGuid);
                if (growSetting == null)
                {
                    return new RepositoryActionResult<GrowPhaseSetting>(entity, RepositoryActionStatus.NotFound);
                }
                if (growSetting.UserGuid != currentUserGuid)
                {
                    return new RepositoryActionResult<GrowPhaseSetting>(entity, RepositoryActionStatus.NotFound);
                }

                _dbContext.GrowPhaseSetting.Add(entity);
                int result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<GrowPhaseSetting>(entity, RepositoryActionStatus.Created);
                }
                return new RepositoryActionResult<GrowPhaseSetting>(entity, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<GrowPhaseSetting> DeleteGrowPhaseSetting(int growSettingId, int GrowPhaseSettingId,
            Guid currentUserGuid)
        {
            try
            {
                GrowPhaseSetting GrowPhaseSetting =
                    _dbContext.GrowPhaseSetting.FirstOrDefault(ug => ug.GrowPhaseSettingId == GrowPhaseSettingId
                                                                  && ug.GrowSetting.UserGuid == currentUserGuid
                                                                  && ug.GrowSettingId == growSettingId);

                if (GrowPhaseSetting == null)
                {
                    return new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.NotFound);
                }

                // This edit phase order of all phases so that it stays in sync after delete of a phase
                List<GrowPhaseSetting> incorrectGrowPhaseSettings =
                    _dbContext.GrowPhaseSetting.Where(
                        ug => ug.PhaseOrder > GrowPhaseSetting.PhaseOrder && ug.GrowSettingId == GrowPhaseSetting.GrowSettingId)
                        .ToList();

                _dbContext.GrowPhaseSetting.Remove(GrowPhaseSetting);

                foreach (GrowPhaseSetting entity in incorrectGrowPhaseSettings)
                {
                    entity.PhaseOrder -= 1;
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                return new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.Deleted);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        private bool GrowPhaseSettingExists(int id)
        {
            return _dbContext.GrowPhaseSetting.Count(e => e.GrowPhaseSettingId == id) > 0;
        }

        #endregion

        #region FanSettings

        public FanSetting GetFanSetting(int growSettingId, int GrowPhaseSettingId, int fanSettingsId, Guid currentUserGuid)
        {
            return _dbContext.FanSetting.FirstOrDefault(ug =>
                ug.GrowPhaseSetting.GrowSetting.UserGuid == currentUserGuid &&
                ug.GrowPhaseSettingId == GrowPhaseSettingId &&
                ug.FanSettingId == fanSettingsId);
        }

        public RepositoryActionResult<FanSetting> PutFanSetting(int fanSettingId, FanSetting modifiedUserGrow,
            Guid currentUserGuid)
        {
            if (modifiedUserGrow.GrowPhaseSetting.GrowSetting.UserGuid != currentUserGuid)
            {
                return new RepositoryActionResult<FanSetting>(modifiedUserGrow, RepositoryActionStatus.NotFound);
            }

            //_dbContext.Entry(modifiedUserGrow).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!FanSettingExists(fanSettingId))
                {
                    return new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.Error, ex);
                }
                throw;
            }
            return new RepositoryActionResult<FanSetting>(modifiedUserGrow, RepositoryActionStatus.Updated);
        }

        public RepositoryActionResult<FanSetting> PostFanSetting(FanSetting entity, Guid currentUserGuid)
        {
            try
            {
                GrowSetting growSetting = GetGrowSetting(entity.GrowPhaseSetting.GrowSettingId, currentUserGuid);
                if (growSetting == null)
                {
                    return new RepositoryActionResult<FanSetting>(entity, RepositoryActionStatus.NotFound);
                }

                bool currentFanSetting =
                    _dbContext.FanSetting.Any(fs => fs.GrowPhaseSettingId == entity.GrowPhaseSetting.GrowPhaseSettingId);
                if (currentFanSetting)
                {
                    return new RepositoryActionResult<FanSetting>(entity, RepositoryActionStatus.Error);
                }

                _dbContext.FanSetting.Add(entity);
                int result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<FanSetting>(entity, RepositoryActionStatus.Created);
                }
                return new RepositoryActionResult<FanSetting>(entity, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<FanSetting> DeleteFanSetting(int growSettingId, int fanSettingId,
            Guid currentUserGuid)
        {
            try
            {
                FanSetting fanSetting = _dbContext.FanSetting.FirstOrDefault(ug => ug.FanSettingId == fanSettingId
                                                                                   &&
                                                                                   ug.GrowPhaseSetting.GrowSetting.UserGuid ==
                                                                                   currentUserGuid
                                                                                   &&
                                                                                   ug.GrowPhaseSetting.GrowSetting.GrowSettingId ==
                                                                                   growSettingId);

                if (fanSetting == null)
                {
                    return new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.NotFound);
                }

                _dbContext.FanSetting.Remove(fanSetting);

                _dbContext.SaveChanges();
                return new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.Deleted);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        private bool FanSettingExists(int id)
        {
            return _dbContext.FanSetting.Count(e => e.FanSettingId == id) > 0;
        }

        #endregion

        #region LightSettings

        public List<LightSetting> GetLightSettings(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid)
        {
            return
                _dbContext.LightSetting.Where(
                    ls => ls.GrowPhaseSettingId == GrowPhaseSettingId && ls.GrowPhaseSetting.GrowSettingId == growSettingId).ToList();
        }

        public LightSetting GetLightSetting(int growSettingId, int GrowPhaseSettingId, int lightSettingsId,
            Guid currentUserGuid)
        {
            return
                _dbContext.LightSetting.FirstOrDefault(
                    ls =>
                        ls.GrowPhaseSetting.GrowSettingId == growSettingId && ls.GrowPhaseSettingId == GrowPhaseSettingId &&
                        ls.LightSettingId == lightSettingsId);
        }

        public RepositoryActionResult<LightSetting> PutLightSetting(int growSettingId, int GrowPhaseSettingId,
            int lightSettingsId,
            LightSetting modLightSetting, Guid currentUserGuid)
        {
            if (modLightSetting.GrowPhaseSetting.GrowSetting.UserGuid != currentUserGuid)
            {
                return new RepositoryActionResult<LightSetting>(modLightSetting, RepositoryActionStatus.NotFound);
            }

            //_dbContext.Entry(modLightSetting).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!LightSettingExists(lightSettingsId))
                {
                    return new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.Error, ex);
                }
                throw;
            }
            return new RepositoryActionResult<LightSetting>(modLightSetting, RepositoryActionStatus.Updated);
        }

        public RepositoryActionResult<LightSetting> PostLightSetting(LightSetting entity, Guid currentUserGuid)
        {
            try
            {
                GrowSetting growSetting = GetGrowSetting(entity.GrowPhaseSetting.GrowSettingId, currentUserGuid);
                if (growSetting == null)
                {
                    return new RepositoryActionResult<LightSetting>(entity, RepositoryActionStatus.NotFound);
                }
                if (growSetting.UserGuid != currentUserGuid)
                {
                    return new RepositoryActionResult<LightSetting>(entity, RepositoryActionStatus.NotFound);
                }

                _dbContext.LightSetting.Add(entity);
                int result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<LightSetting>(entity, RepositoryActionStatus.Created);
                }
                return new RepositoryActionResult<LightSetting>(entity, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<LightSetting> DeleteLightSetting(int growSettingId, int GrowPhaseSettingId,
            int lightSettingsId, Guid currentUserGuid)
        {
            try
            {
                LightSetting lightSetting =
                    _dbContext.LightSetting.FirstOrDefault(ls => ls.GrowPhaseSetting.GrowSettingId == growSettingId
                                                                 && ls.GrowPhaseSettingId == GrowPhaseSettingId
                                                                 && ls.LightSettingId == lightSettingsId
                                                                 &&
                                                                 ls.GrowPhaseSetting.GrowSetting.UserGuid ==
                                                                 currentUserGuid);

                if (lightSetting == null)
                {
                    return new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.NotFound);
                }

                _dbContext.LightSetting.Remove(lightSetting);

                _dbContext.SaveChanges();
                return new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.Deleted);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        private bool LightSettingExists(int id)
        {
            return _dbContext.LightSetting.Count(e => e.LightSettingId == id) > 0;
        }

        #endregion

        #region WaterSettings

        public List<WaterSetting> GetWaterSettings(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid)
        {
            return
                _dbContext.WaterSetting.Where(
                    ls => ls.GrowPhaseSettingId == GrowPhaseSettingId && ls.GrowPhaseSetting.GrowSettingId == growSettingId).ToList();
        }

        public WaterSetting GetWaterSetting(int growSettingId, int GrowPhaseSettingId, int waterSettingId,
            Guid currentUserGuid)
        {
            return
                _dbContext.WaterSetting.FirstOrDefault(
                    ls =>
                        ls.GrowPhaseSetting.GrowSettingId == growSettingId && ls.GrowPhaseSettingId == GrowPhaseSettingId &&
                        ls.WaterSettingId == waterSettingId);
        }

        public RepositoryActionResult<WaterSetting> PutWaterSetting(int growSettingId, int GrowPhaseSettingId,
            int waterSettingId,
            WaterSetting modifiedWaterSetting, Guid currentUserGuid)
        {
            if (modifiedWaterSetting.GrowPhaseSetting.GrowSetting.UserGuid != currentUserGuid)
            {
                return new RepositoryActionResult<WaterSetting>(modifiedWaterSetting, RepositoryActionStatus.NotFound);
            }

            //_dbContext.Entry(modifiedWaterSetting).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!WaterSettingExists(waterSettingId))
                {
                    return new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.Error, ex);
                }
                throw;
            }
            return new RepositoryActionResult<WaterSetting>(modifiedWaterSetting, RepositoryActionStatus.Updated);
        }

        public RepositoryActionResult<WaterSetting> PostWaterSetting(WaterSetting entity, Guid currentUserGuid)
        {
            try
            {
                GrowSetting growSetting = GetGrowSetting(entity.GrowPhaseSetting.GrowSettingId, currentUserGuid);
                if (growSetting == null)
                {
                    return new RepositoryActionResult<WaterSetting>(entity, RepositoryActionStatus.NotFound);
                }
                if (growSetting.UserGuid != currentUserGuid)
                {
                    return new RepositoryActionResult<WaterSetting>(entity, RepositoryActionStatus.NotFound);
                }

                _dbContext.WaterSetting.Add(entity);
                int result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<WaterSetting>(entity, RepositoryActionStatus.Created);
                }
                return new RepositoryActionResult<WaterSetting>(entity, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<WaterSetting> DeleteWaterSetting(int growSettingId, int GrowPhaseSettingId,
            int waterSettingId, Guid currentUserGuid)
        {
            try
            {
                WaterSetting waterSetting =
                    _dbContext.WaterSetting.FirstOrDefault(ls => ls.GrowPhaseSetting.GrowSettingId == growSettingId
                                                                 && ls.GrowPhaseSettingId == GrowPhaseSettingId
                                                                 && ls.WaterSettingId == waterSettingId
                                                                 &&
                                                                 ls.GrowPhaseSetting.GrowSetting.UserGuid ==
                                                                 currentUserGuid);

                if (waterSetting == null)
                {
                    return new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.NotFound);
                }

                _dbContext.WaterSetting.Remove(waterSetting);

                _dbContext.SaveChanges();
                return new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.Deleted);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.Error, ex);
            }
        }

        private bool WaterSettingExists(int id)
        {
            return _dbContext.WaterSetting.Count(e => e.WaterSettingId == id) > 0;
        }

        #endregion
    }
}