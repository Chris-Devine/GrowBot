using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowBot.API.Entities.GrowSettings
{
    public class LightSetting
    {
        public int LightSettingId { get; set; }
        public Int64 TurnOnLightTimeTicks { get; set; }
        public Int64 TurnOffLightTimeTicks { get; set; }
        public int GrowPhaseSettingId { get; set; }
        public virtual GrowPhaseSetting GrowPhaseSetting { get; set; }


        [NotMapped]
        public TimeSpan TurnOnLightTime
        {
            get { return TimeSpan.FromTicks(TurnOnLightTimeTicks); }
            set { TurnOnLightTimeTicks = value.Ticks; }
        }

        [NotMapped]
        public TimeSpan TurnOffLightTime
        {
            get { return TimeSpan.FromTicks(TurnOffLightTimeTicks); }
            set { TurnOffLightTimeTicks = value.Ticks; }
        }
    }
}