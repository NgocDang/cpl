using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionIndexViewModel
    {
        public int? SysUserId { get; set; }
        public decimal? TokenAmount { get; set; }
        public List<PricePredictionTab> PricePredictionTabs { get; set; }
    }

    public class PricePredictionTab
    {
        public int Id { get; set; }
        public DateTime ResultTime { get; set; }
        public DateTime ToBeComparedTime { get; set; }
    }
}
