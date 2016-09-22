using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings
{
    public class FanSettingFactory : IFanSettingFactory
    {
        public FanSetting PostFanSetting(FanSettingDto fanSetting)
        {
            return new FanSetting
            {
                FanSettingId = fanSetting.FanSettingId,
                MaxHeatCelsius = fanSetting.MaxHeatCelsius,
                MinHeatCelsius = fanSetting.MinHeatCelsius,
                MinFanSpeedPercent = fanSetting.MinFanSpeedPercent
            };
        }

        public FanSettingDto GetFanSetting(FanSetting fanSetting)
        {
            return new FanSettingDto
            {
                FanSettingId = fanSetting.FanSettingId,
                MaxHeatCelsius = fanSetting.MaxHeatCelsius,
                MinHeatCelsius = fanSetting.MinHeatCelsius,
                MinFanSpeedPercent = fanSetting.MinFanSpeedPercent,
                GrowPhaseSettingId = fanSetting.GrowPhaseSettingId
            };
        }

        public FanSetting PutFanSetting(FanSetting originalEntity, FanSettingDto fanSetting)
        {
            originalEntity.MaxHeatCelsius = fanSetting.MaxHeatCelsius;
            originalEntity.MinHeatCelsius = fanSetting.MinHeatCelsius;
            originalEntity.MinFanSpeedPercent = fanSetting.MinFanSpeedPercent;
            return originalEntity;
        }
    }
}