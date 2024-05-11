using PeanutTradeTestTask.BLL.Models;

namespace PeanutTradeTestTask.BLL.Interfaces
{
    public interface IExchange
    {
        Task<RateModel> GetRate(string baseCurrency, string quoteCurrency);
    }
}
