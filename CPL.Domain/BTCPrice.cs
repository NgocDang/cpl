using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CPL.Domain
{
    public class BTCPrice : Entity
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public long Time { get; set; }
    }
}
