using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Service;
using ExchangeRateUpdater.Api;
using ExchangeRateUpdater.Presentation.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddServiceLayer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
