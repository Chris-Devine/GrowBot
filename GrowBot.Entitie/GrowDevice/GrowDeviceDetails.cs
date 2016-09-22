using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowBot.API.Entities.GrowDevice
{
    class GrowDeviceDetails
    {
        public int DeviceDetailsId { get; set; }
        public GrowDeviceType DeviceTypeId { get; set; }
        public string DeviceSerial { get; set; }

        
    }
}
