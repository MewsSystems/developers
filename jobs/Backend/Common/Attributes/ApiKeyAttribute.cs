using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private string ApiKeyName = "exchangerates-api-key";

        public async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
            {
                context.Result = new BadRequestObjectResult("Invalid api key!");
                return;
            }

            var appSettings =
                context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings.GetSection("ApiKey").Value;
            if (apiKey == null || !extractedApiKey.Equals(apiKey))
            {
                context.Result = new BadRequestObjectResult("Invalid api key!");
                return;
            }
            
            await next();
        }
    }
}
