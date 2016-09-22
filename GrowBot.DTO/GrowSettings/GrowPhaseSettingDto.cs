using System.Collections.Generic;

namespace GrowBot.DTO.GrowSettings
{
    public class GrowPhaseSettingDto
    {
        public int GrowPhaseSettingId { get; set; }
        public string GrowPhaseName { get; set; }
        public int Duration { get; set; }
        public string PhaseNotes { get; set; }
        public int PhaseOrder { get; set; }
        public virtual GrowSettingDto UserGrow { get; set; }
        public virtual ICollection<FanSettingDto> FanSetting { get; set; }
        public virtual ICollection<LightSettingDto> LightSetting { get; set; }
        public virtual ICollection<WaterSettingDto> WaterSetting { get; set; }
        public int GrowSettingId { get; set; }
    }
}