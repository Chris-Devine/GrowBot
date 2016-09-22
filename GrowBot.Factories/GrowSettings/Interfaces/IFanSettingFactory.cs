using GrowBot.API.Entities.GrowSettings;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings.Interfaces
{
    public interface IFanSettingFactory
    {
        FanSetting PostFanSetting(FanSettingDto fanSetting);
        FanSettingDto GetFanSetting(FanSetting fanSetting);
        FanSetting PutFanSetting(FanSetting originalEntity, FanSettingDto fanSetting);
    }
}