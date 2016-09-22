using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrowBot.API.Entities.GrowSettings;

namespace GrowBot.API.Entities.GrowResults
{
    public class Grow
    {

        public int GrowId { get; set; }

        [MaxLength(4000)]
        public string GrowName { get; set; }

        [MaxLength]
        public string GrowNotes { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        public Guid UserGuid { get; set; }

        public bool SystemDefaultGrow { get; set; }
        public bool PublicGrow { get; set; }
        public int GrowSettingId { get; set; }
        public virtual GrowSetting GrowSetting { get; set; }
    }
}
