using System;
using System.Linq;
using GrowBot.API.Entities.GrowResults;
using GrowBot.API.Factories.GrowResults.Interfaces;
using GrowBot.API.Factories.GrowSettings;
using GrowBot.DTO.GrowResults;

namespace GrowBot.API.Factories.GrowResults
{
    class GrowFactory : IGrowFactory
    {
        private readonly GrowSettingFactory _growSettingFactory;

        public GrowFactory(GrowSettingFactory growSettingFactory)
        {
            _growSettingFactory = growSettingFactory;
        }

        public Grow PostGrow(GrowDto grow, Guid userGuid)
        {
            return new Grow
            {
                GrowName = grow.GrowName,
                GrowNotes = grow.GrowNotes,
                CreateDateTime = DateTime.UtcNow,
                SystemDefaultGrow = false,
                PublicGrow = false,
                UserGuid = userGuid
            };
        }

        public GrowDto GetGrow(Grow grow)
        {
            return new GrowDto
            {
                GrowId = grow.GrowId,
                GrowName = grow.GrowName,
                GrowNotes = grow.GrowNotes,
                CreateDateTime = grow.CreateDateTime,
                SystemDefaultGrow = grow.SystemDefaultGrow,
                PublicGrow = grow.PublicGrow,
                GrowSetting = _growSettingFactory.GetGrowSetting(grow.GrowSetting)
            };
        }

        public Grow PutGrow(Grow originalEntity, GrowDto grow)
        {
            originalEntity.GrowName = grow.GrowName;
            originalEntity.GrowNotes = grow.GrowNotes;
            originalEntity.PublicGrow = grow.PublicGrow;
            return originalEntity;
        }
    }
}