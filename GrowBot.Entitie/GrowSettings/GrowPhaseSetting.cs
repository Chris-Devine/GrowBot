using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowBot.API.Entities.GrowSettings
{
    public class GrowPhaseSetting
    {
        public GrowPhaseSetting()
        {
            FanSetting = new HashSet<FanSetting>();
            LightSetting = new HashSet<LightSetting>();
            WaterSetting = new HashSet<WaterSetting>();
        }

        public int GrowPhaseSettingId { get; set; }

        [MaxLength(4000)]
        public string GrowPhaseName { get; set; }
        public int Duration { get; set; }
        [MaxLength]
        public string PhaseNotes { get; set; }
        public int PhaseOrder { get; set; }
        public int GrowSettingId { get; set; }
        public virtual GrowSetting GrowSetting { get; set; }
        public virtual ICollection<FanSetting> FanSetting { get; set; }
        public virtual ICollection<LightSetting> LightSetting { get; set; }
        public virtual ICollection<WaterSetting> WaterSetting { get; set; }
    }
}