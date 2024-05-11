using Microsoft.AspNetCore.Mvc;
using PeanutTradeTestTask.API.Attributes;
using PeanutTradeTestTask.BLL.Interfaces;

namespace PeanutTradeTestTask.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CryptoExchangeController : ControllerBase
    {
        private readonly ILogger<CryptoExchangeController> _logger;
        private readonly ICryptoExchangeSevice _cryptoExchangeSevice;

        public CryptoExchangeController(ILogger<CryptoExchangeController> logger, ICryptoExchangeSevice cryptoExchangeSevice)
        {
            _logger = logger;
            _cryptoExchangeSevice = cryptoExchangeSevice;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates([Currency] string baseCurrency, [Currency] string quoteCurrency)
        {
            var rates = await _cryptoExchangeSevice.GetRates(baseCurrency, quoteCurrency);

            return Ok(rates);
        }

        [HttpGet]
        public async Task<IActionResult> Estimate(decimal inputAmount, [Currency] string inputCurrency, [Currency] string outputCurrency)
        {
            var estimate = await _cryptoExchangeSevice.Estimate(inputAmount, inputCurrency, outputCurrency);

            return Ok(estimate);
        }
    }
}
