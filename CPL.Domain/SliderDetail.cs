using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class SliderDetail : Entity
    {
        public int Id { get; set; }
        public string DesktopImage { get; set; }
        public string MobileImage { get; set; }
        public int LangId { get; set; }
        public int SliderId { get; set; }

        public virtual Lang Lang { get; set; }
        public virtual Slider Slider { get; set; }
    }
}
