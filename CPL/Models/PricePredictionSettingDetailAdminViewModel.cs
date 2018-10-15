using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class PricePredictionSettingDetailAdminViewModel
    {
        public int Id { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

        public int PricePredictionSettingId { get; set; }
    }
}
