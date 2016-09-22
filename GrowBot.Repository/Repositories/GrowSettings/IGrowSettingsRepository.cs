using System;
using System.Collections.Generic;
using GrowBot.API.Entities.GrowSettings;

namespace GrowBot.API.Repository.Repositories.GrowSettings
{
    public interface IGrowSettingsRepository
    {
        #region UserGrows

        List<GrowSetting> GetGrowSettings(bool publicGrows, Guid currentUserGuid);
        GrowSetting GetGrowSetting(int growSettingId, Guid currentUserGuid);
        RepositoryActionResult<GrowSetting> PutGrowSetting(int growSettingId, GrowSetting modifiedGrowSetting, Guid currentUserGuid);
        RepositoryActionResult<GrowSetting> PostGrowSetting(GrowSetting entity);
        RepositoryActionResult<GrowSetting> DeleteGrowSetting(int growSettingId, Guid currentUserGuid);

        #endregion

        #region GrowPhaseSetting

        List<GrowPhaseSetting> GetGrowPhaseSettings(int growSettingId, Guid currentUserGuid);
        GrowPhaseSetting GetGrowPhaseSetting(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid);

        RepositoryActionResult<GrowPhaseSetting> PutGrowPhaseSetting(int GrowPhaseSettingId, GrowPhaseSetting modifiedUserGrow,
            Guid currentUserGuid);

        RepositoryActionResult<GrowPhaseSetting> PostGrowPhaseSetting(GrowPhaseSetting entity, Guid currentUserGuid);

        RepositoryActionResult<GrowPhaseSetting> DeleteGrowPhaseSetting(int growSettingId, int GrowPhaseSettingId,
            Guid currentUserGuid);

        #endregion

        #region FanSettings

        FanSetting GetFanSetting(int growSettingId, int GrowPhaseSettingId, int fanSettingsId, Guid currentUserGuid);

        RepositoryActionResult<FanSetting> PutFanSetting(int fanSettingsId, FanSetting modifiedGrowPhaseSetting,
            Guid currentUserGuid);

        RepositoryActionResult<FanSetting> PostFanSetting(FanSetting entity, Guid currentUserGuid);
        RepositoryActionResult<FanSetting> DeleteFanSetting(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid);

        #endregion

        #region LightSettings

        List<LightSetting> GetLightSettings(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid);
        LightSetting GetLightSetting(int growSettingId, int GrowPhaseSettingId, int lightSettingsId, Guid currentUserGuid);

        RepositoryActionResult<LightSetting> PutLightSetting(int growSettingId, int GrowPhaseSettingId, int lightSettingsId,
            LightSetting modifiedLightSetting, Guid currentUserGuid);

        RepositoryActionResult<LightSetting> PostLightSetting(LightSetting entity, Guid currentUserGuid);

        RepositoryActionResult<LightSetting> DeleteLightSetting(int growSettingId, int GrowPhaseSettingId, int lightSettingsId,
            Guid currentUserGuid);

        #endregion

        #region WaterSettings

        List<WaterSetting> GetWaterSettings(int growSettingId, int GrowPhaseSettingId, Guid currentUserGuid);
        WaterSetting GetWaterSetting(int growSettingId, int GrowPhaseSettingId, int waterSettingId, Guid currentUserGuid);

        RepositoryActionResult<WaterSetting> PutWaterSetting(int growSettingId, int GrowPhaseSettingId, int waterSettingId,
            WaterSetting modifiedWaterSetting, Guid currentUserGuid);

        RepositoryActionResult<WaterSetting> PostWaterSetting(WaterSetting entity, Guid currentUserGuid);

        RepositoryActionResult<WaterSetting> DeleteWaterSetting(int growSettingId, int GrowPhaseSettingId, int waterSettingId,
            Guid currentUserGuid);

        #endregion
    }
}