using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CPL.Common.BTCRateHelper
{
    public class Rate
    {
        public decimal Value { get; set; }
        public DateTime Time { get; set; }
    }

    public static class BTCRateHelper
    {
        public static Rate GetBTCRate(string currenciesPair)
        {
            // URL from hitBTC:
            // https://api.hitbtc.com/api/2/public/ticker/BTCUSD
            // URL from Binance:
            // https://api.binance.com//api/v3/ticker/price?symbol=BTCUSDT

            var response = new HttpClient().GetAsync("https://api.binance.com//api/v3/ticker/price?symbol=" + currenciesPair);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var jsonResponse = response.Result.Content.ReadAsStringAsync();
                return new Rate { Value = (decimal)JObject.Parse(jsonResponse.Result)["price"] , Time = DateTime.Now };
            }
            return null;
        }
    }
}
