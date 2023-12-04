using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCases;
using Serilog;
using Adapter.ExchangeRateProvider.CzechNatBank;
using ExchangeRateUpdater.Host.WebApi.Configuration;

namespace ExchangeRateUpdater.Host.WebApi
{
    public class ApplicationHostBuilder
    {

        private const string ApplicationName = "ExchangeRateUpdater";
        private readonly ISettings _settings;
        private readonly Serilog.ILogger _logger;

        public ApplicationHostBuilder(ISettings? settings, Serilog.ILogger? logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IHostBuilder Configure()
        {
            var applicationBuilder = new HostBuilder().ConfigureWebHost(webBuilder =>
            {
                ConfigureServices(webBuilder);
            }).UseSerilog();


            return applicationBuilder;
        }

        protected virtual void ConfigureServices(IWebHostBuilder webBuilder)
        {
            webBuilder
                .ConfigureServices(services =>
                {
                    RegisterUseCases(services);
                    RegisterAdapters(services);
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddSerilog(_logger);
                    services.AddMvcCore();
                });
            webBuilder.UseKestrel();
            webBuilder.Configure(applicationBuilder =>
            {
                if (_settings.EnableSwagger)
                {
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    applicationBuilder.UseSwagger();
                    applicationBuilder.UseSwaggerUI(_ =>
                    {
                        _.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate Update API");
                        _.RoutePrefix = string.Empty;
                    });
                }
                
                applicationBuilder.UseRouting();
                applicationBuilder.UseHttpsRedirection();
                applicationBuilder.UseAuthorization();
                
                applicationBuilder.UseEndpoints(configuration =>
                {
                    configuration.MapControllers();
                });
            });
        }

        protected virtual void RegisterAdapters(IServiceCollection services)
        {
            services.AddHttpClient($"{ApplicationName}-http-client",
            client =>
            {
                // Set the base address of the named client.
                client.BaseAddress = new Uri(_settings.CzechNationalBankBaseAddress);
            });
            services.AddSingleton<IExchangeRateProviderRepository, CzechNationalBankRepository>();
        }

        private void RegisterUseCases(IServiceCollection services)
        {
            services.AddSingleton<ExchangeUseCase>();
        }


    }
}
