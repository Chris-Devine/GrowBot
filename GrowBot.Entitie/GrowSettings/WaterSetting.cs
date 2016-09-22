using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowBot.API.Entities.GrowSettings
{
    public class WaterSetting
    {
        public int WaterSettingId { get; set; }
        public Int64 TurnOnWaterTimeTicks { get; set; }
        public Int64 TurnOffWaterTimeTicks { get; set; }
        public int GrowPhaseSettingId { get; set; }
        public virtual GrowPhaseSetting GrowPhaseSetting { get; set; }


        [NotMapped]
        public TimeSpan TurnOnWaterTime
        {
            get { return TimeSpan.FromTicks(TurnOnWaterTimeTicks); }
            set { TurnOnWaterTimeTicks = value.Ticks; }
        }

        [NotMapped]
        public TimeSpan TurnOffWaterTime
        {
            get { return TimeSpan.FromTicks(TurnOffWaterTimeTicks); }
            set { TurnOffWaterTimeTicks = value.Ticks; }
        }
    }
}