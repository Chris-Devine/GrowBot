using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowBot.API.Entities.GrowSettings;

namespace GrowBot.API.Entities.GrowDevice
{
    class GrowDevice
    {
        public int GrowDeviceId { get; set; }
        public GrowDeviceDetails GrowDeviceTypeId { get; set; }
        // ID of current owner
        public Guid UserGuid { get; set; }
        // Current User Grow Settings Asigned to Device
        //public Grow CurrentGrowId { get; set; }

    }
}
