using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Rate : Entity
    {
        public int Id { get; set; }
        public float Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }
    }
}
