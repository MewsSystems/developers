using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            string apiBaseUrl = builder.Configuration.GetValue<string>("CnbApi:BaseUrl")
                ?? throw new InvalidOperationException("Required configuration CnbApi:BaseUrl not defined");

            builder.Services.AddHostedService<ExchangeRateProvider>();
            builder.Services.AddHttpClient<ICnbApiClient, CnbApiClient>(c =>
            {
                c.BaseAddress = new Uri(apiBaseUrl);
            });

            var host = builder.Build();
            host.Run();
        }
    }
}