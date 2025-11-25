using EchangeRateUpdater.Api.Configurations.Extensions;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Serilog;

InitializeBootstrapLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Services.AddControllers();
		
	builder.Services.AddApiServices()
		.AddApplicationServices()
		.AddInfrastructure(builder.Configuration)
		.AddControllers();

	var app = builder.Build();
	
	app.UseExceptionHandler();
	app.UseHealthChecks("/health");
	
	
	app.UseHttpsRedirection();
	app.UseApiDocumentation();
	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Log.Error(ex, "Unhandled exception");
}
finally
{
	await Log.CloseAndFlushAsync();
}

return;


void InitializeBootstrapLogger()
{ 
	var config = new LoggerConfiguration().WriteTo.Console(); 
        
	Log.Logger = config.CreateBootstrapLogger();
}