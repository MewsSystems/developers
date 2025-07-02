using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;

namespace ExchangeRateUpdater.Host
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen(c =>
			{
				// Set the comments path for the Swagger JSON and UI.
				foreach (var xmlFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
				{
					c.IncludeXmlComments(xmlFile);
				}
			});

			builder.Services.Configure<ApiBehaviorOptions>
				(options => options.SuppressInferBindingSourcesForParameters = true);

			builder.Services.RegisterServices();

			var app = builder.Build();

			app.RegisterMiddlewares();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
