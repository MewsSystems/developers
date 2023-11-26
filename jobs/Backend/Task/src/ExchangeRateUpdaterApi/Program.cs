using ExchangeRateUpdaterApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog.Core;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace ExchangeRateUpdaterApi
{
    public static class Program
    {
        public const string ApplicationName = "MewsChallenge - ExchangeRateUpdaterApi";

        private static IHost _host;

        public static void Main(string[] args)
        {
            Logger logger = SerilogConfiguration.Create(ApplicationName);
            logger.Information("Logger created;");
            
            var container = new Container();

            var hostBuilder = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
                    builder.ConfigureServices(services =>
                    {
                        services.AddControllers();
                        
                        services.AddSimpleInjector(container, options =>
                        {
                            options
                                .AddAspNetCore()
                                .AddControllerActivation();
                        });
                        
                        services.AddSwaggerGen(options =>
                        {
                            options.SwaggerDoc("v1", new OpenApiInfo
                            {
                                Title = ApplicationName,
                                Version = "v1"
                            });
                        });
                    }).Configure(application =>
                    {
                        application.UseSwagger();
                        application.UseSwaggerUI(options =>
                        {
                            options.SwaggerEndpoint("/swagger/v1/swagger.json", ApplicationName);
                        });

                        application.UseRouting();

                        application.UseAuthorization();

                        application.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    })
                );

            _host = hostBuilder.Build().UseSimpleInjector(container);

            container.RegisterInstance(logger);

            _host.Run();

            /*private static IEnumerable<Currency> currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };*/

            /*try
            {
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();*/
        }
    }
}
