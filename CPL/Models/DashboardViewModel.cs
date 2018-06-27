using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class DashboardViewModel
    {
        public double TotalBalance { get; set; }
        public double CPLBalanc { get; set; }
        public double BTCBalance { get; set; }
        public double ETHBalance { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string GameType { get; set; }
        public double Amount { get; set; }
        public bool Win { get; set; }
        public bool Loss { get; set; }
        public double Balance { get; set; }
        public double Bonus { get; set; }
    }
}
