using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePredictionSettingDetail : Entity
    {
        public int Id { get; set; }
        public int LangId { get; set; }
        public string Title { get; set; }

        public int PricePredictionSettingId { get; set; }

        public virtual Lang Lang { get; set; }
        public virtual PricePredictionSetting PricePredictionSetting { get; set; }
    }
}
