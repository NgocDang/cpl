using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Group : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }

        public virtual ICollection<Slider> Sliders { get; set; }
        public virtual ICollection<FAQ> FAQs { get; set; }
    }
}
