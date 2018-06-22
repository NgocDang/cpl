using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class HomeViewModel
    {
        //Languages
        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }

        //Team section
        public IList<TeamViewModel> Teams { get; set; }
    }
}
