using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPL.Misc.Utils
{
    public static class CoinExchangeExtension
    {
        public static decimal CoinExchanging()
        {
            decimal rate = 0;
            var response = new HttpClient().GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=ETHBTC");
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var jsonResponse = response.Result.Content.ReadAsStringAsync();
                rate = (decimal)JObject.Parse(jsonResponse.Result)["price"];
                return rate;
            }
            return -1;
        }

        public static decimal BTCCurrentPrice(string currency)
        {
            // URL from hitBTC:
            // https://api.hitbtc.com/api/2/public/ticker/BTCUSD
            // URL from Binance:
            // https://api.binance.com//api/v3/ticker/price?symbol=BTCUSDT

            var response = new HttpClient().GetAsync(string.Format("https://api.binance.com//api/v3/ticker/price?symbol=BTC{0}", currency));
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var jsonResponse = response.Result.Content.ReadAsStringAsync();
                return (decimal)JObject.Parse(jsonResponse.Result)["price"];
            }
            return -1;
        }
    }
}
