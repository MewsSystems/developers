using ExchangeRate.Application;
using ExchangeRate.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddMemoryCache()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHttpClient()
    .AddProblemDetails()
    .AddControllers();

builder.Services.AddApiVersioning();


// register layer services
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();