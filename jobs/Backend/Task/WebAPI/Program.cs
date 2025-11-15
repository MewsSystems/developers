using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.Sources;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.SetBasePath(builder.Environment.ContentRootPath)
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - simple permissive policy (adjust for production)
const string CorsPolicyName = "DefaultCorsPolicy";
builder.Services.AddCors(options =>
{
	options.AddPolicy(CorsPolicyName, policy =>
	{
		if (builder.Environment.IsDevelopment())
		{
			policy
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
		}
		else
		{
			policy
				.WithOrigins(
					"https://example.com",
					"https://yourfrontend.com"
				)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		}
	});
});

builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddTransient<IExchangeRateProvider, CnbExchangeRateProvider>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// HTTPS redirection
app.UseHttpsRedirection();

// Exception handling / diagnostics
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler(errorApp =>
	{
		errorApp.Run(async context =>
		{
			context.Response.ContentType = "application/json";
			var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
			var problem = Results.Problem(detail: exceptionHandler?.Error?.Message, title: "An error occurred");
			await problem.ExecuteAsync(context);
		});
	});
}

// Swagger (UI)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.RoutePrefix = "swagger";
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "ExchangeRateUpdater API V1");
});

// Routing, CORS, Auth
app.UseRouting();
app.UseCors(CorsPolicyName);
//Enabled, when requested!
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.MapFallback(() => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", environment = app.Environment.EnvironmentName }));

app.Run();
