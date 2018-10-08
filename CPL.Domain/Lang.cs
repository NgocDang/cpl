using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Lang : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public virtual ICollection<LangDetail> LangDetails { get; set; }
        public virtual ICollection<LangMsgDetail> LangMsgDetails { get; set; }
        public virtual ICollection<MobileLangDetail> MobileLangDetails { get; set; }
        public virtual ICollection<MobileLangMsgDetail> MobileLangMsgDetails { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<LotteryDetail> LotteryDetails { get; set; }
        public virtual ICollection<SliderDetail> SliderDetails { get; set; }
		public virtual ICollection<PricePredictionSettingDetail> PricePredictionSettingDetails { get; set; }
		public virtual ICollection<PricePredictionDetail> PricePredictionDetails { get; set; }
        public virtual ICollection<PricePredictionCategoryDetail> PricePredictionCategoryDetails { get; set; }
        public virtual ICollection<FAQ> FAQs { get; set; }
    }
}
