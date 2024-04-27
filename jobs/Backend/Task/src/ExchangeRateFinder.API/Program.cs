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

app.MapControllers();

var updateCZKExchangeRateData = app.Services.GetRequiredService<IUpdateCZKExchangeRateDataService>();

// Update CZK exchange rate data on startup
updateCZKExchangeRateData.UpdateDataAsync();

// Schedule the update of CZK exchange rate data to run daily at 14:35 PM
// It is mentioned in their website that the data is updated daily at 14:30 PM
ScheduleUpdateService(updateCZKExchangeRateData);

app.Run();

void ScheduleUpdateService(IUpdateCZKExchangeRateDataService service)
{
    var timeUntilNextRun = GetTimeUntilNextRun();

    var scheduler = new Timer(async (state) =>
    {
        await service.UpdateDataAsync();
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
