namespace GrowBot.DTO.GrowSettings
{
    public class FanSettingDto
    {
        public int FanSettingId { get; set; }
        public int MaxHeatCelsius { get; set; }
        public int MinHeatCelsius { get; set; }
        public int MinFanSpeedPercent { get; set; }
        public virtual GrowPhaseSettingDto GrowPhaseSetting { get; set; }
        public int GrowPhaseSettingId { get; set; }
    }
}