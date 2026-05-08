using ExchangeRateUpdater.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Exchange Rate API V1");
    options.RoutePrefix = "swagger";
});

app.UseGlobalExceptionHandling();
app.UseHttpsRedirection();
app.MapControllers();
app.MapOpenApi();

app.Run();

public partial class Program { }


