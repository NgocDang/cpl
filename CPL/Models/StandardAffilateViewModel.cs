using CPL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class StandardAffliateViewModel
    {
        public StandardAffliateViewModel()
        {
            //Tier2DirectSale = new List<int>();
            //Tier3DirectSale = new List<int>();
            //DirectSale = new List<int>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal TotalSale { get; set; }

        public decimal TotalIntroducer { get; set; }

        public DateTime? AffiliateCreatedDate { get; set; }
        public string Action { get; set; }
        public int? Tier1DirectRate { get; set; }
        public int? Tier2SaleToTier1Rate { get; set; }
        public int? Tier3SaleToTier1Rate { get; set; }
        public bool? IsBlocked { get; set; }

        //public List<int> DirectSale { get; set; }
        //public List<int> Tier2DirectSale { get; set; }
        //public List<int> Tier3DirectSale { get; set; }

        public decimal TotalDirectCPLUsed { get; set; } // Lottery + Priceprediction
        public decimal TotalTier2DirectCPLUsed { get; set; }
        public decimal TotalTier3DirectCPLUsed { get; set; }

        public decimal TotalDirectCPLAwarded { get; set; }
        public decimal TotalTier2DirectCPLAwarded { get; set; }
        public decimal TotalTier3DirectCPLAwarded { get; set; }

        public string AffiliateCreatedDateInString { get; set; }

    }
}
