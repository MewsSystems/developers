using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Application.Abstractions;
using ExchangeRateUpdater.CnbProvider;
using ExchangeRateUpdater.CnbProvider.Abstractions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<ICnbRateProvider, CnbRateProvider>();
builder.Services.AddScoped<ICnbRateProviderClient, CnbRateProviderClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
