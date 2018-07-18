using CPL.WCF.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CPL.WCF.ExchangeCurrentPrice
{
    #region BTC

    [DataContract]
    public class SetBTCCurrentPriceResult
    {
        [DataMember]
        public Status Status { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public long DateTime { get; set; }
    }

    [DataContract]
    public class GetBTCCurrentPriceResult
    {
        [DataMember]
        public Status Status { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public long DateTime { get; set; }
    }

    #endregion
}