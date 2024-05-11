using Newtonsoft.Json.Linq;
using PeanutTradeTestTask.BLL.Constants;
using PeanutTradeTestTask.BLL.Interfaces;
using PeanutTradeTestTask.BLL.Models;

namespace PeanutTradeTestTask.BLL.Exchanges
{
    public class BinanceExchange : IExchange
    {
        private const string ExchangeName = "Binance";

        public async Task<RateModel> GetRate(string baseCurrency, string quoteCurrency)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri(BaseApiUrls.Binance) };
            var response = await httpClient.GetAsync($"/api/v3/depth?symbol={baseCurrency}{quoteCurrency}&limit=1");
            if (response.IsSuccessStatusCode)
            {
                var sellOrderBook = JObject.Parse(await response.Content.ReadAsStringAsync());

                return new()
                {
                    ExchangeName = ExchangeName,
                    Rate = sellOrderBook["asks"]?[0]?[0]?.Value<decimal>() ?? 0
                };
            }

            response = await httpClient.GetAsync($"/api/v3/depth?symbol={quoteCurrency}{baseCurrency}&limit=1");
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Invalid pair");
            }

            var buyOrderBook = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new()
            {
                ExchangeName = ExchangeName,
                Rate = (1 / buyOrderBook["bids"]?[0]?[0]?.Value<decimal>()) ?? 0
            };
        }
    }
}
