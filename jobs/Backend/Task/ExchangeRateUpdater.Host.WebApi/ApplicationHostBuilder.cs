using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCases;
using Serilog;
using Microsoft.Extensions.Http;
using System.Reflection.PortableExecutable;
using System.Diagnostics;
using Adapter.ExchangeRateProvider.CzechNationalBank;

namespace ExchangeRateUpdater.Host.WebApi
{
    public class ApplicationHostBuilder
    {

        private const string ApplicationName = "ExchangeRateUpdater";
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
                    services.AddHttpClient($"{ApplicationName}-http-client",
                    client =>
                    {
                        // Set the base address of the named client.
                        client.BaseAddress = new Uri("https://www.cnb.cz/en/");
                    });
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddSerilog(Log.Logger);
                    services.AddMvcCore();
                });
            webBuilder.UseKestrel();
            webBuilder.Configure(applicationBuilder =>
            {
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                applicationBuilder.UseSwagger();
                applicationBuilder.UseSwaggerUI();

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
            services.AddSingleton<IExchangeRateProviderRepository, CzechNationalBankRepository>();
        }

        private void RegisterUseCases(IServiceCollection services)
        {
            services.AddSingleton<BuyOrderUseCase>();
        }


    }
}
