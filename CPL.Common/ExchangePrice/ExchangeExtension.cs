using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CPL.Common.ExchangePrice
{
    public static class ExchangeExtension
    {
        public static decimal GetBTCPrice()
        {
            // URL from hitBTC:
            // https://api.hitbtc.com/api/2/public/ticker/BTCUSD
            // URL from Binance:
            // https://api.binance.com//api/v3/ticker/price?symbol=BTCUSDT

            var response = new HttpClient().GetAsync("https://api.binance.com//api/v3/ticker/price?symbol=BTCUSDT");
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
