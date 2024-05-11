using Newtonsoft.Json.Linq;
using PeanutTradeTestTask.BLL.Constants;
using PeanutTradeTestTask.BLL.Interfaces;
using PeanutTradeTestTask.BLL.Models;

namespace PeanutTradeTestTask.BLL.Exchanges
{
    public class KucoinExchange : IExchange
    {
        private const string ExchangeName = "Kucoin";

        public async Task<RateModel> GetRate(string baseCurrency, string quoteCurrency)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri(BaseApiUrls.Kucoin) };
            var response = await httpClient.GetAsync($"/api/v1/market/orderbook/level2_20?symbol={baseCurrency}-{quoteCurrency}");
            var body = await response.Content.ReadAsStringAsync();
            if (body != null)
            {
                var sellOrderBook = JObject.Parse(body);
                if (!string.IsNullOrEmpty(sellOrderBook["data"]?["asks"]?.ToString()))
                {
                    return new()
                    {
                        ExchangeName = ExchangeName,
                        Rate = sellOrderBook["data"]?["asks"]?[0]?[0]?.Value<decimal>() ?? 0
                    };
                }
            }

            response = await httpClient.GetAsync($"/api/v1/market/orderbook/level2_20?symbol={quoteCurrency}-{baseCurrency}");
            body = await response.Content.ReadAsStringAsync();
            if (body == null)
            {
                throw new ArgumentException("Invalid pair");
            }

            var buyOrderBook = JObject.Parse(body);
            if (string.IsNullOrEmpty(buyOrderBook["data"]?["asks"]?.ToString()))
            {
                throw new ArgumentException("Invalid pair");
            }

            return new()
            {
                ExchangeName = ExchangeName,
                Rate = (1 / buyOrderBook["data"]?["bids"]?[0]?[0]?.Value<decimal>()) ?? 0
            };
        }
    }
}
