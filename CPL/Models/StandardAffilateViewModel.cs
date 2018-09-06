using CPL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class StandardAffliateViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public decimal TotalIntroducer { get; set; }

        public DateTime? AffiliateCreatedDate { get; set; }
        public string Action { get; set; }
        public int? Tier1DirectRate { get; set; }
        public int? Tier2SaleToTier1Rate { get; set; }
        public int? Tier3SaleToTier1Rate { get; set; }
        public bool? IsBlocked { get; set; }

        // Lottey game
        public decimal? TotalDirectCPLUsedInLottery { get; set; }
        public decimal? TotalTier2DirectCPLUsedInLottery { get; set; }
        public decimal? TotalTier3DirectCPLUsedInLottery { get; set; }
        public decimal? TotalDirectCPLAwardedInLottery { get; set; }
        public decimal? TotalTier2DirectCPLAwardedInLottery { get; set; }
        public decimal? TotalTier3DirectCPLAwardedInLottery { get; set; }

        // price prediction game
        public decimal? TotalDirectCPLUsedInPricePrediction { get; set; }
        public decimal? TotalTier2DirectCPLUsedInPricePrediction { get; set; }
        public decimal? TotalTier3DirectCPLUsedInPricePrediction { get; set; }
        public decimal? TotalDirectCPLAwardedInPricePrediction { get; set; }
        public decimal? TotalTier2DirectCPLAwardedInPricePrediction { get; set; }
        public decimal? TotalTier3DirectCPLAwardedInPricePrediction { get; set; }

        public decimal? TotalSale
        {
            get
            {
                var total = 
                    // Total sale in lottery
                    TotalDirectCPLUsedInLottery + TotalTier2DirectCPLUsedInLottery + TotalTier3DirectCPLUsedInLottery
                       - TotalDirectCPLAwardedInLottery - TotalTier2DirectCPLAwardedInLottery - TotalTier3DirectCPLAwardedInLottery
                    // Total sale in price prediction
                       + TotalDirectCPLUsedInPricePrediction + TotalTier2DirectCPLUsedInPricePrediction + TotalTier3DirectCPLUsedInPricePrediction
                       - TotalDirectCPLAwardedInPricePrediction - TotalTier2DirectCPLAwardedInPricePrediction - TotalTier3DirectCPLAwardedInPricePrediction;

                return (total > 0) ? total : 0; 
            }
        }

        // variable in string
        public string AffiliateCreatedDateInString { get; set; }
    }
}
