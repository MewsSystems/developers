using ExchangeRateLayer.BLL.Services.Abstract;

namespace ExchangeRateLayer.BLL.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IExchangeRateService> _exchangeRateService;

        public ServiceManager()
        {
            _exchangeRateService = new Lazy<IExchangeRateService>(() => new ExchangeRateService());
        }

        public IExchangeRateService ExchangeRateService => _exchangeRateService.Value;
    }
}
