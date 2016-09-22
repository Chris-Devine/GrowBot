using GrowBot.API.Entities.GrowSettings;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings.Interfaces
{
    public interface IWaterSettingFactory
    {
        WaterSetting PostWaterSetting(WaterSettingDto waterSetting);
        WaterSettingDto GetWaterSetting(WaterSetting waterSetting);
        WaterSetting PutWaterSetting(WaterSetting originalEntity, WaterSettingDto waterSetting);
    }
}