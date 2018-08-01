using CPL.WCF.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CPL.WCF.BTC
{
    [DataContract]
    public class BTCNotifyResult
    {
        [DataMember]
        public Status Status { get; set; }
    }
}