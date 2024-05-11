using PeanutTradeTestTask.BLL.Models;

namespace PeanutTradeTestTask.BLL.Interfaces
{
    public interface ICryptoExchangeSevice
    {
        Task<RateModel[]> GetRates(string baseCurrency, string quoteCurrency);
        Task<EstimateModel> Estimate(decimal inputAmount, string inputCurrency, string outputCurrency);
    }
}
