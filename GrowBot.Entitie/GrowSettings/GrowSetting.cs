using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrowBot.API.Entities.GrowResults;

namespace GrowBot.API.Entities.GrowSettings
{
    public class GrowSetting
    {
        //TODO: CHANGE THIS TO USER GROW SETTINGS AS I WILL NEED TO CREATE A SIMILAR USER GROW CLASS THAT CALLS SETTINGS INSTEAD
        public GrowSetting()
        {
            GrowPhaseSetting = new HashSet<GrowPhaseSetting>();
        }

        public int GrowSettingId { get; set; }
        public Guid UserGuid { get; set; }

        [MaxLength(4000)]
        public string GrowSettingName { get; set; }
        [MaxLength]
        public string GrowSettingNotes { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        public bool SystemDefaultGrowSetting { get; set; }

        public virtual ICollection<GrowPhaseSetting> GrowPhaseSetting { get; set; }
    }
}