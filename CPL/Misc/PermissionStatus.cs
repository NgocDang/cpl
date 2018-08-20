using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public class PermissionStatus
    {
        public int Code { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }

        public const int OkCode = 0;
        public const string OkText = "Success";

        public const int UnLoggedInCode = OkCode + 1;
        public const string UnLoggedInText = "Error, you are not logged in yet!";
        public const string UnLoggedInUrl = "/Authentication/LogIn";

        public const int UnAuthorizedCode = UnLoggedInCode + 1;
        public const string UnAuthorizedText = "Error, you are not authorized to do this!";
        public const string UnAuthorizedUrl = "/Home/Error403";

        public const int ExceptionCode = UnAuthorizedCode + 1;
        public const string ExceptionText = "";
        public const string ExceptionUrl = "/Home/Error";
    }
}
