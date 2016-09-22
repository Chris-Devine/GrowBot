using System;
using System.Collections.Generic;

namespace GrowBot.DTO.GrowSettings
{
    public class GrowSettingDto
    {
        public int GrowSettingId { get; set; }
        public string GrowSettingName { get; set; }
        public string GrowSettingNotes { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool SystemDefaultGrowSetting { get; set; }
        public virtual ICollection<GrowPhaseSettingDto> GrowPhaseSetting { get; set; }
    }
}