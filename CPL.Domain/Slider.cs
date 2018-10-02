using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Slider : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int GroupId { get; set; }
        public int Status { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<SliderDetail> SliderDetails { get; set; }
    }
}
