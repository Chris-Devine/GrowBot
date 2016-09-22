using System.Linq;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings
{
    public class GrowPhaseSettingFactory : IGrowPhaseSettingFactory
    {
        private readonly FanSettingFactory _fanSettingFactory;
        private readonly LightSettingFactory _lightSettingFactory;
        private readonly WaterSettingFactory _waterSettingFactory;

        public GrowPhaseSettingFactory(FanSettingFactory fanSettingFactory, LightSettingFactory lightSettingFactory,
            WaterSettingFactory waterSettingFactory)
        {
            _fanSettingFactory = fanSettingFactory;
            _lightSettingFactory = lightSettingFactory;
            _waterSettingFactory = waterSettingFactory;
        }

        public GrowPhaseSetting PostGrowPhaseSetting(GrowPhaseSettingDto GrowPhaseSetting)
        {
            return new GrowPhaseSetting
            {
                GrowPhaseName = GrowPhaseSetting.GrowPhaseName,
                Duration = GrowPhaseSetting.Duration,
                PhaseNotes = GrowPhaseSetting.GrowPhaseName,
                GrowSettingId = GrowPhaseSetting.GrowSettingId,
                PhaseOrder = GrowPhaseSetting.PhaseOrder
            };
        }

        public GrowPhaseSettingDto GetGrowPhaseSetting(GrowPhaseSetting GrowPhaseSetting)
        {
            return new GrowPhaseSettingDto
            {
                GrowPhaseSettingId = GrowPhaseSetting.GrowPhaseSettingId,
                GrowPhaseName = GrowPhaseSetting.GrowPhaseName,
                Duration = GrowPhaseSetting.Duration,
                PhaseNotes = GrowPhaseSetting.GrowPhaseName,
                PhaseOrder = GrowPhaseSetting.PhaseOrder,
                WaterSetting = GrowPhaseSetting.WaterSetting.Select(e => _waterSettingFactory.GetWaterSetting(e)).ToList(),
                FanSetting = GrowPhaseSetting.FanSetting.Select(e => _fanSettingFactory.GetFanSetting(e)).ToList(),
                LightSetting = GrowPhaseSetting.LightSetting.Select(e => _lightSettingFactory.GetLightSetting(e)).ToList(),
                GrowSettingId = GrowPhaseSetting.GrowSettingId
            };
        }

        public GrowPhaseSetting PutGrowPhaseSetting(GrowPhaseSetting originalEntity, GrowPhaseSettingDto GrowPhaseSetting)
        {
            originalEntity.GrowPhaseName = GrowPhaseSetting.GrowPhaseName;
            originalEntity.Duration = GrowPhaseSetting.Duration;
            originalEntity.PhaseNotes = GrowPhaseSetting.PhaseNotes;
            //originalEntity.PhaseOrder = GrowPhaseSetting.PhaseOrder; Need to figure a way so that user cant fuck it up if they mess around with fiddler etc
            return originalEntity;
        }
    }
}