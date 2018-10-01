using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class FooterViewModel
    {
        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
    }
}
