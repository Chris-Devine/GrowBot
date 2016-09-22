using System;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings
{
    public class WaterSettingFactory : IWaterSettingFactory
    {
        public WaterSetting PostWaterSetting(WaterSettingDto waterSetting)
        {
            return new WaterSetting
            {
                WaterSettingId = waterSetting.WaterSettingId,
                TurnOnWaterTime = waterSetting.TurnOnWatertTime,
                TurnOffWaterTime = waterSetting.TurnOffWaterTime,
                GrowPhaseSettingId = waterSetting.GrowPhaseSettingId
            };
        }

        public WaterSettingDto GetWaterSetting(WaterSetting waterSetting)
        {
            try
            {
                return new WaterSettingDto
                {
                    WaterSettingId = waterSetting.WaterSettingId,
                    TurnOnWatertTime = waterSetting.TurnOnWaterTime,
                    TurnOffWaterTime = waterSetting.TurnOffWaterTime,
                    GrowPhaseSettingId = waterSetting.GrowPhaseSettingId
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public WaterSetting PutWaterSetting(WaterSetting originalEntity, WaterSettingDto waterSetting)
        {
            originalEntity.TurnOffWaterTime = waterSetting.TurnOffWaterTime;
            originalEntity.TurnOnWaterTime = waterSetting.TurnOnWatertTime;
            return originalEntity;
        }
    }
}