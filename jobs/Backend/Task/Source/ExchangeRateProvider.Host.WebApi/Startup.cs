namespace ExchangeRateProvider.Host.WebApi;

using Application.Interfaces;
using Application.Services;
using Domain.Options;
using Infrasctructure.Clients;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CnbApiOptions>(_configuration.GetSection("CnbApiOptions"));
        services.AddMemoryCache();
        services.AddSingleton<IExchangeRateProvider, CnbExchangeRateProvider>();
        services.AddSingleton<IExchangeRateService, ExchangeRateService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exchange Rate API", Version = "v1" });
        });

        var cnbApiOptions = _configuration.GetSection("CnbApiOptions").Get<CnbApiOptions>();
        if (cnbApiOptions != null)
            services.AddHttpClient<CnbCzClient>((sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<CnbApiOptions>>().Value;
                    client.BaseAddress = new Uri(options.BaseUrl);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(cnbApiOptions.HandlerLifetimeMinutes))
                .AddPolicyHandler(GetRetryPolicy(cnbApiOptions));
        services.AddHostedService<ExchangeRateBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate API v1"));
        }

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(CnbApiOptions options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(options.RetryCount, _ =>
                TimeSpan.FromMilliseconds(options.RetryDelayMilliseconds));
    }
}
