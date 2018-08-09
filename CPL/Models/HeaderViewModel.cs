using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class HeaderViewModel
    {
        public SysUserViewModel User { get; set; }
        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
        public NewsViewModel News {get;set;}
    }
}
