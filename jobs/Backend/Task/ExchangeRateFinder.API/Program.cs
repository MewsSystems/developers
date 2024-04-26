using ExchangeRateFinder.Application;
using ExchangeRateFinder.Application.Configuration;
using ExchangeRateFinder.Application.Extensions;
using ExchangeRateFinder.Infrastructure.Extensions;
using ExchangeRateSyncService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CBNConfiguration>(builder.Configuration.GetSection(nameof(CBNConfiguration)));

// Add services to the container.
builder.Services.AddDomain();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Get the update service
var updateCZKExchangeRateData = app.Services.GetRequiredService<IUpdateCZKExchangeRateDataService>();

// Update CZK exchange rate data on startup
updateCZKExchangeRateData.UpdateDataAsync();

// Schedule the update of CZK exchange rate data to run daily at 14:30 PM
// This is when it is update 

ScheduleUpdateService(updateCZKExchangeRateData);

app.Run();

// Function to schedule the update service
void ScheduleUpdateService(IUpdateCZKExchangeRateDataService service)
{
    // Get the time until the next run
    var timeUntilNextRun = GetTimeUntilNextRun();

    // Set up a timer to run the service
    var scheduler = new Timer(async (state) =>
    {
        await service.UpdateDataAsync();
    }, null, timeUntilNextRun, TimeSpan.FromHours(24)); // Repeat every 24 hours
}

// Function to calculate the time until the next run at 2:30 AM
TimeSpan GetTimeUntilNextRun()
{
    var now = DateTime.Now;
    var nextRun = new DateTime(now.Year, now.Month, now.Day, 14, 30, 0);
    if (now > nextRun)
    {
        nextRun = nextRun.AddDays(1);
    }
    return nextRun - now;
}
