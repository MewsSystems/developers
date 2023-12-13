using Microsoft.Extensions.Logging.ApplicationInsights;

namespace ExchangeRateUpdater.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = 100_000_000;
                    });

                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureLogging((hostingContext, builder) =>
                    {
                        builder
                        .AddApplicationInsights()
                        .AddFilter<ApplicationInsightsLoggerProvider>("SimpleLogger", LogLevel.Debug)
                        .SetMinimumLevel(LogLevel.Warning);
                    });
                });
    }
}
