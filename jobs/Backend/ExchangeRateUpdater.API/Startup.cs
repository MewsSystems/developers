using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.ClientFactories;
using ExchangeRateUpdater.DependencyResolution;

namespace ExchangeRateUpdater.API
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddMvc();
            services.AddConfigurationSections(_configuration);
            services.AddExternalApiRegistrations();
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetExchangeRatesQuery).Assembly));
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app) 
        {
            app.UseHsts();

            app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => 
                {
                    endpoints.MapControllers();
                })
                .UseSwagger()
                .UseSwaggerUI();
        }
    }
}
