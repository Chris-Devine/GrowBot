using System;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings.Interfaces
{
    public interface IGrowSettingFactory
    {
        GrowSetting PostGrowSetting(GrowSettingDto userGrow, Guid userGuid);
        GrowSettingDto GetGrowSetting(GrowSetting grow);
        GrowSetting PutGrowSetting(GrowSetting originalEntity, GrowSettingDto userGrow);
    }
}