using System;

namespace GrowBot.DTO.GrowSettings
{
    public class WaterSettingDto
    {
        public int WaterSettingId { get; set; }
        public TimeSpan TurnOnWatertTime { get; set; }
        public TimeSpan TurnOffWaterTime { get; set; }
        public virtual GrowPhaseSettingDto GrowPhaseSetting { get; set; }
        public int GrowPhaseSettingId { get; set; }
    }
}