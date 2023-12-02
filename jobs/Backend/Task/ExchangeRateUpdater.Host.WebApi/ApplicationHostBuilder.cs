using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Ports;
using Serilog;
using System.Reflection.PortableExecutable;

namespace ExchangeRateUpdater.Host.WebApi
{
    public class ApplicationHostBuilder
    {
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
                    ConfigureDependencies(services);
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddLogging();
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

        protected virtual void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateProviderRepository>(new ExchangeRateProviderRepositoryInMemory());
        }
    }
}
