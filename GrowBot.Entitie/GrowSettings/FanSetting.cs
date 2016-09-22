namespace GrowBot.API.Entities.GrowSettings
{
    public class FanSetting
    {
        public int FanSettingId { get; set; }
        public int MaxHeatCelsius { get; set; }
        public int MinHeatCelsius { get; set; }
        public int MinFanSpeedPercent { get; set; }
        public int GrowPhaseSettingId { get; set; }
        public virtual GrowPhaseSetting GrowPhaseSetting { get; set; }
    }
}