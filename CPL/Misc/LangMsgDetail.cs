using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public static class LangMsgDetailHelper
    {
        public static IList<LangMsgDetailViewModel> LangMsgDetails { get; set; }

        public static string Get(int langId, string name)
        {
            return LangMsgDetails.FirstOrDefault(x => x.LangId == langId && x.Name == name).Value;
        }
    }
}
