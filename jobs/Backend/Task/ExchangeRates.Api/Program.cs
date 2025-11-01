namespace ExchangeRates.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            Configuration.ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            Configuration.Configure(app, app.Environment);

            app.Run();
        }
    }
}
