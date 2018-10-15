using System;
using System.Collections.Generic;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Models
{
    public class PricePredictionSettingAdminViewModel
    {
        public PricePredictionSettingAdminViewModel()
        {
            PricePredictionSettingDetails = new List<PricePredictionSettingDetailAdminViewModel>();
            PricePredictionCategories = new List<PricePredictionCategoryAdminViewModel>();
        }

        public int Id { get; set; }

        public TimeSpan OpenBettingTime { get; set; }
        public TimeSpan CloseBettingTime { get; set; }
        public int HoldingTimeInterval { get; set; }
        public int ResultTimeInterval { get; set; }
        public int DividendRate { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public int PricePredictionCategoryId { get; set; }

        public string BettingTimeInString { get; set; }
		
        public List<PricePredictionSettingDetailAdminViewModel> PricePredictionSettingDetails { get; set; }
        public List<PricePredictionCategoryAdminViewModel> PricePredictionCategories { get; set; }

        public string CreateDateInString
        {
            get
            {
                return CreatedDate.ToString(Format.DateTime);
            }
        }
    }
}
