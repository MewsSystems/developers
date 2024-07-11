using Czech_National_Bank_ExchangeRates.Infrastructure;
using Czech_National_Bank_ExchangeRates.Models;
using Czech_National_Bank_ExchangeRates.Repository;
using ExchangeRateUpdater;
using Microsoft.Extensions.Options;

namespace Czech_National_Bank_ExchangeRates
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddSingleton<ICNBExchangeRateConnection>(p =>
              p.GetRequiredService<IOptions<CNBExchangeRateConnection>>().Value);

            var cnbExchangeRateConfig = builder.Configuration.GetSection("ExchangeRateApiConnection");
            builder.Services.Configure<CNBExchangeRateConnection>(cnbExchangeRateConfig);
            builder.Services.AddTransient<ICNBExchangeRateRepo, CNBExchangeRateRepo>();
            builder.Services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
            builder.Services.AddTransient<IHttpClientService, HttpClientService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}