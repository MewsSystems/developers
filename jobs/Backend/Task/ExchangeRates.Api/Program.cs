using ExchangeRates.Api.Extensions;

namespace ExchangeRates.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();

            builder.Services.AddExchangeRatesServices(builder.Configuration);

            var app = builder.Build();

            app.ConfigureApp(builder.Environment);

            app.Run();
        }
    }
}
