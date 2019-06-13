using Mews.Backend.Task.Api.Configs;
using Mews.Backend.Task.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.Backend.Task.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var exchangeUrl = Configuration.GetValue<string>("EXCHANGE_URL");
            services.AddSingleton<IExchageRateParser>(new TextFileExchangeRateParser(exchangeUrl));
            services.AddTransient<IExchangeRateProvider, CzechBankRateProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsConfig.SetupCors);
            app.UseMvc();
        }
    }
}
