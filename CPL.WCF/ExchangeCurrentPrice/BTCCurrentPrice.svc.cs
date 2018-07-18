using CPL.WCF.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CPL.WCF.ExchangeCurrentPrice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BTCCurrentPrice" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BTCCurrentPrice.svc or BTCCurrentPrice.svc.cs at the Solution Explorer and start debugging.
    public class BTCCurrentPrice : IBTCCurrentPrice
    {
        /// <summary>
        /// Gets the BTC current price.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public GetBTCCurrentPriceResult GetBTCCurrentPrice()
        {
            try
            {
                return new GetBTCCurrentPriceResult
                {
                    Status = new Status { Code = Status.OkCode, Text = Status.OkText },
                    Price = CPL.WCF.Misc.BTCCurrentPrice.Price,
                    DateTime = ((DateTimeOffset)CPL.WCF.Misc.BTCCurrentPrice.Time).ToUnixTimeSeconds()
                };
            }
            catch(Exception ex)
            {
                return new GetBTCCurrentPriceResult { Status = new Status { Code = Status.ExceptionCode, Text = ex.Message } };
            }
        }

        /// <summary>
        /// Sets the BTC current price.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SetBTCCurrentPriceResult SetBTCCurrentPrice()
        {
            try
            {
                var currentPrice = CPL.Common.ExchangePrice.ExchangeCurrentPrice.BTCCurrentPrice();
                CPL.WCF.Misc.BTCCurrentPrice.Price = currentPrice.Price;
                CPL.WCF.Misc.BTCCurrentPrice.Time = currentPrice.Time;
                return new SetBTCCurrentPriceResult
                {
                    Status = new Status { Code = Status.OkCode, Text = Status.OkText },
                    Price = CPL.WCF.Misc.BTCCurrentPrice.Price,
                    DateTime = ((DateTimeOffset)CPL.WCF.Misc.BTCCurrentPrice.Time).ToUnixTimeSeconds()
                };
            }
            catch (Exception ex)
            {
                return new SetBTCCurrentPriceResult { Status = new Status { Code = Status.ExceptionCode, Text = ex.Message } };
            }
        }
    }
}
