using ExchangeRateService.AutoRegistration;
using ExchangeRateService.Services;

namespace ExchangeRateService.CustomRegistration;

internal class ExchangeRateServices : IServices
{
    public void Register(IServiceCollection services)
    {
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
    }
}