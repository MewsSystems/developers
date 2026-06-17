using Mews.ExchangeRateProvider.Application.Utils;
using Mews.ExchangeRateProvider.Infrastructure.Utils;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOptions();
builder.Services.ApplicationServices();
builder.Services.InfrastructureServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1.0",
        new OpenApiInfo()
        {
            Title = "MEWS API for Czech National Bank daily exchange rates",
            Version = "1.0"
        });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MEWS API for Czech National Bank daily exchange rates v1.0")
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
