using PeanutTradeTestTask.BLL.Interfaces;
using PeanutTradeTestTask.BLL.Models;

namespace PeanutTradeTestTask.BLL.Services
{
    public class CryptoExchangeSevice : ICryptoExchangeSevice
    {
        private readonly IEnumerable<IExchange> _exchanges;

        public CryptoExchangeSevice(IEnumerable<IExchange> exchanges)
        {
            _exchanges = exchanges;
        }

        public Task<RateModel[]> GetRates(string baseCurrency, string quoteCurrency)
        {
            var tasks = new List<Task<RateModel>>();
            foreach (var exchange in _exchanges)
            {
                tasks.Add(exchange.GetRate(baseCurrency, quoteCurrency));
            }

            return Task.WhenAll(tasks).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw task.Exception;
                }

                var results = task.Result;
                return results;
            });
        }

        public async Task<EstimateModel> Estimate(decimal inputAmount, string inputCurrency, string outputCurrency)
        {
            var rates = await GetRates(inputCurrency, outputCurrency);
            var bestRate = rates.OrderBy(x => x.Rate).First();

            return new()
            {
                ExchangeName = bestRate.ExchangeName,
                OutputAmount = bestRate.Rate * inputAmount
            };
        }
    }
}
