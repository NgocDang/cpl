using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CPL.WCF.ExchangeCurrentPrice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBTCCurrentPrice" in both code and config file together.
    [ServiceContract]
    public interface IBTCCurrentPrice
    {
        /// <summary>
        /// Sets the BTC current price.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        SetBTCCurrentPriceResult SetBTCCurrentPrice();

        /// <summary>
        /// Gets the BTC current price.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GetBTCCurrentPriceResult GetBTCCurrentPrice();
    }
}
