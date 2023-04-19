using ExchangeRateUpdater.WebApi.Services;
using ExchangeRateUpdater.WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExchangeRatesGetter, ExchangeRatesDownloader>();
builder.Services.AddScoped<IExchangeRatesParser, CnbExchangeRatesFormatParser>();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<IExchangeRatesDownloaderFromURL, ExchangeRatesDownloaderFromURL>();
builder.Services.AddHttpClient<ExchangeRatesDownloaderFromURL>();

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

app.Run();
