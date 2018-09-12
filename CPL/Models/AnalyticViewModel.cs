using CPL.Misc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PageViewsViewModel
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class BounceRateViewModel
    {
        public DateTime Date { get; set; }
        public double Rate { get; set; }
    }

    public class DeviceCategoryViewModel
    {
        public DateTime Date { get; set; }
        public EnumDeviceCategory DeviceCategory { get; set; }
        public int Count { get; set; }
    }

}
