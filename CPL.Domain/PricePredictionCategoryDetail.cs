using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class PricePredictionCategoryDetail : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int LangId { get; set; }
        public int PricePredictionCategoryId { get; set; }

        public virtual Lang Lang { get; set; }
        public virtual PricePredictionCategory PricePredictionCategory { get; set; }
    }
}
