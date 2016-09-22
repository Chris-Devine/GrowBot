using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowBot.DTO.GrowSettings;

namespace GrowBot.DTO.GrowResults
{
    public class GrowDto
    {
        public int GrowId { get; set; }
        public string GrowName { get; set; }
        public string GrowNotes { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool SystemDefaultGrow { get; set; }
        public virtual GrowSettingDto GrowSetting { get; set; }
        public bool PublicGrow { get; set; }
    }
}