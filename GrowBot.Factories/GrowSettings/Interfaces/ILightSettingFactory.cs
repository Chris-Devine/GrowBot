using GrowBot.API.Entities.GrowSettings;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings.Interfaces
{
    public interface ILightSettingFactory
    {
        LightSetting PostLightSetting(LightSettingDto lightSetting);
        LightSettingDto GetLightSetting(LightSetting lightSetting);
        LightSetting PutLightSetting(LightSetting originalEntity, LightSettingDto lightSetting);
    }
}