using ExchangeRateUpdater.Infrastructure.BusinessLogic;
using ExchangeRateUpdater.Model.Configuration;
using Microsoft.OpenApi.Models;
using ICnbConfig = ExchangeRateUpdater.Model.Configuration.ICnbConfig;

namespace ExchangeRateUpdater.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ICnbConfig _cnbConfig;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _cnbConfig = configuration.Get<CnbConfig>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exchange Rate Updater",
                    Version = "v1",
                    Description = "An ASP.NET Core Web Api for getting the daily exchange rates for currencies."
                });
            });
            services.AddTransient<ExchangeRateProvider, ExchangeRateProvider>();
            services.AddTransient((s) => _cnbConfig);
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("./v1/swagger.json", "Exchange Rate Updater");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
