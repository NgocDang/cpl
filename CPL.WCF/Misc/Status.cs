using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CPL.WCF.Misc
{
    [DataContract]
    public class Status
    {
        public const int OkCode = 0;
        public const string OkText = "Success";

        public const int InvalidIxIdCode = OkCode + 1;
        public const string InvalidIxIdText = "TxId is Invalid!";

        public const int ExceptionCode = InvalidIxIdCode + 1;
        public const string ExceptionText = "";

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}