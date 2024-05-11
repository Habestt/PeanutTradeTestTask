using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PeanutTradeTestTask.BLL.Models;

namespace PeanutTradeTestTask.API.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class CurrencyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var parameter in context.ActionDescriptor.Parameters.Cast<ControllerParameterDescriptor>())
            {
                var hasAttribute = parameter.ParameterInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(CurrencyAttribute));
                if (hasAttribute)
                {
                    context.ActionArguments.TryGetValue(parameter.Name, out object? argument);
                    var cryptocurrencyExists = Enum.TryParse(argument?.ToString(), true, out CryptocurrencyTypes currency);
                    if (!cryptocurrencyExists)
                    {
                        context.Result = new BadRequestObjectResult("Invalid currency.");
                        return;
                    }

                    context.ActionArguments[parameter.Name] = currency.ToString().ToUpper();
                }
            }

            await next();
        }
    }

}
