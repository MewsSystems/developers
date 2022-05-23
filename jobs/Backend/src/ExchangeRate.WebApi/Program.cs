using System.Text.Json.Serialization;
using ExchangeRate.Service.Extensions;
using ExchangeRate.WebApi.Middleware;
using ExchangeRate.WebApi.Models;
using Framework.Logging.Extensions;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// add logging
builder.Services.AddFrameworkLogging(builder.Configuration.GetSection("Logging"));
// add feature management => see appsettings.json => section FeatureManagement
builder.Services.AddFeatureManagement();
// register exchange rate service with all dependencies
builder.Services.AddExchangeRateService(builder.Configuration);

// add API specific
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Exchange rate API",
		Description = "ASP.NET Core Web API. Gets exchange rate data from Czech National Bank."
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseCors(b =>
	{
		b.SetIsOriginAllowed(_ => true);
		b.AllowAnyHeader();
		b.AllowAnyMethod();
	});
}

// add middlewares
app.UseMiddlewareForFeature<CorrelationIdMiddleware>(nameof(FeatureFlags.EnableCorrelationIdMiddleware));
app.UseMiddlewareForFeature<ExceptionHandlingMiddleware>(nameof(FeatureFlags.EnableExceptionHandlingMiddleware));

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange rate API");
	c.RoutePrefix = app.Environment.IsDevelopment() ? "swagger" : string.Empty;
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHealthChecks("/health");
});

app.Run();
