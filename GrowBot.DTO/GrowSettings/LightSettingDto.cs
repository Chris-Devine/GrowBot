using System;

namespace GrowBot.DTO.GrowSettings
{
    public class LightSettingDto
    {
        public int LightSettingId { get; set; }
        public TimeSpan TurnOnLightTime { get; set; }
        public TimeSpan TurnOffLightTime { get; set; }
        public virtual GrowPhaseSettingDto GrowPhaseSetting { get; set; }
        public int GrowPhaseSettingId { get; set; }
    }
}