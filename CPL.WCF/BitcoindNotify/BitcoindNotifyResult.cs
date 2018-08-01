using CPL.WCF.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CPL.WCF.BitcoindNotify
{
    [DataContract]
    public class BitcoindNotifyTransactionIdResult
    {
        [DataMember]
        public Status Status { get; set; }
    }
}