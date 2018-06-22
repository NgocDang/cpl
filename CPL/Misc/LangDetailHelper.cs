using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public static class LangDetailHelper
    {
        public static IList<LangDetailViewModel> LangDetails { get; set; }

        public static string Get(int langId, string name)
        {
            return LangDetails.FirstOrDefault(x => x.LangId == langId && x.Name == name).Value;
        }
    }
}
