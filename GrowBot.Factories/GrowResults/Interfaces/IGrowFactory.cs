using System;
using GrowBot.API.Entities.GrowResults;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.DTO.GrowResults;
using GrowBot.DTO.GrowSettings;


namespace GrowBot.API.Factories.GrowResults.Interfaces
{
    public interface IGrowFactory
    {
        Grow PostGrow(GrowDto grow, Guid userGuid);
        GrowDto GetGrow(Grow grow);
        Grow PutGrow(Grow originalEntity, GrowDto grow);
    }
}