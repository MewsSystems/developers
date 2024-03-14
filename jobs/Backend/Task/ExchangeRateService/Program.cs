using ExchangeRateService.AutoRegistration;
using ExchangeRateService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(x => x.AddPolicy("Default", policy => policy.AllowAnyOrigin()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// Commented due to missing AI (application insights) connection string
//builder.Services.AddOpenTelemetry().UseAzureMonitor();

builder.Services.AddApplicationServices();
builder.Services.AddDomainApiServices();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.MapApiEndpoints();

app.Run();

public partial class Program { }