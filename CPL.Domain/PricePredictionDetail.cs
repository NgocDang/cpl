using CPL.Common.Models;
using System.Collections.Generic;

namespace CPL.Domain
{
    public class PricePredictionDetail : Entity
    {
        public int Id { get; set; }
        public int LangId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

        public int PricePredictionId { get; set; }

        public virtual Lang Lang { get; set; }
        public virtual PricePrediction PricePrediction { get; set; }
    }
}
