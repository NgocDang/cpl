using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class LotteryCategory : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewId { get; set; }

        public virtual ICollection<Lottery> Lotteries { get; set; }
    }
}
