using System;
using System.Linq;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings
{
    public class GrowSettingFactory : IGrowSettingFactory
    {
        private readonly GrowPhaseSettingFactory _GrowPhaseSettingFactory;

        public GrowSettingFactory(GrowPhaseSettingFactory GrowPhaseSettingFactory)
        {
            _GrowPhaseSettingFactory = GrowPhaseSettingFactory;
        }

        public GrowSetting PostGrowSetting(GrowSettingDto userGrow, Guid userGuid)
        {
            return new GrowSetting
            {
                GrowSettingName = userGrow.GrowSettingName,
                GrowSettingNotes = userGrow.GrowSettingNotes,
                CreateDateTime = DateTime.UtcNow,
                SystemDefaultGrowSetting = false,
                UserGuid = userGuid
            };
        }

        public GrowSettingDto GetGrowSetting(GrowSetting growSetting)
        {
            return new GrowSettingDto
            {
                GrowSettingId = growSetting.GrowSettingId,
                GrowSettingName = growSetting.GrowSettingName,
                GrowSettingNotes = growSetting.GrowSettingNotes,
                CreateDateTime = growSetting.CreateDateTime,
                SystemDefaultGrowSetting = growSetting.SystemDefaultGrowSetting,
                GrowPhaseSetting = growSetting.GrowPhaseSetting.Select(e => _GrowPhaseSettingFactory.GetGrowPhaseSetting(e)).ToList()
            };
        }

        public GrowSetting PutGrowSetting(GrowSetting originalEntity, GrowSettingDto userGrow)
        {
            originalEntity.GrowSettingName = userGrow.GrowSettingName;
            originalEntity.GrowSettingNotes = userGrow.GrowSettingNotes;
            return originalEntity;
        }
    }
}