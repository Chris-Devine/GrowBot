using GrowBot.API.Entities.GrowSettings;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.API.Factories.GrowSettings.Interfaces
{
    public interface IGrowPhaseSettingFactory
    {
        GrowPhaseSetting PostGrowPhaseSetting(GrowPhaseSettingDto GrowPhaseSetting);
        GrowPhaseSettingDto GetGrowPhaseSetting(GrowPhaseSetting GrowPhaseSetting);
        GrowPhaseSetting PutGrowPhaseSetting(GrowPhaseSetting originalEntity, GrowPhaseSettingDto GrowPhaseSetting);
    }
}