using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings
{
    public class LightSettingFactory : ILightSettingFactory
    {
        public LightSetting PostLightSetting(LightSettingDto lightSetting)
        {
            return new LightSetting
            {
                LightSettingId = lightSetting.LightSettingId,
                TurnOnLightTime = lightSetting.TurnOnLightTime,
                TurnOffLightTime = lightSetting.TurnOffLightTime,
                GrowPhaseSettingId = lightSetting.GrowPhaseSettingId
            };
        }

        public LightSettingDto GetLightSetting(LightSetting lightSetting)
        {
            return new LightSettingDto
            {
                LightSettingId = lightSetting.LightSettingId,
                TurnOnLightTime = lightSetting.TurnOnLightTime,
                TurnOffLightTime = lightSetting.TurnOffLightTime,
                GrowPhaseSettingId = lightSetting.GrowPhaseSettingId
            };
        }

        public LightSetting PutLightSetting(LightSetting originalEntity, LightSettingDto lightSetting)
        {
            originalEntity.TurnOffLightTime = lightSetting.TurnOffLightTime;
            originalEntity.TurnOnLightTime = lightSetting.TurnOnLightTime;
            return originalEntity;
        }
    }
}