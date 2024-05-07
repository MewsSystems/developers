using ExchangeRateFinder.API.Extensions;
using ExchangeRateFinder.Application;
using ExchangeRateFinder.Application.Extensions;
using ExchangeRateFinder.Domain.Extensions;
using ExchangeRateFinder.Infrastructure.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.AddApi();
        builder.Services.AddDomain();
        builder.Services.AddInfrastructure();
        builder.Services.AddApplication(configuration);
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        var updateCZKExchangeRateDataService = app.Services.GetRequiredService<IUpdateCZKExchangeRateDataService>();

        // Update CZK exchange rates data on startup
        updateCZKExchangeRateDataService.UpdateDataAsync();

        // Schedule the update of CZK exchange rates data to run daily at 14:35 PM
        // It is mentioned in their website that the data is updated daily at 14:30 PM
        ScheduleUpdateCZKExchangeRateData(updateCZKExchangeRateDataService);

        app.Run();

        void ScheduleUpdateCZKExchangeRateData(IUpdateCZKExchangeRateDataService updateCZKExchangeRateDataService)
        {
            var timeUntilNextRun = GetTimeUntilNextRun();

            var scheduler = new Timer(async (state) =>
            {
                await updateCZKExchangeRateDataService.UpdateDataAsync();
            }, null, timeUntilNextRun, TimeSpan.FromHours(24));
        }

        // Function to calculate the time until the next run at 14:35 PM
        TimeSpan GetTimeUntilNextRun()
        {
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 14, 35, 0);
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }
            return nextRun - now;
        }
    }
}