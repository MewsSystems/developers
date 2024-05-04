using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;
using Microsoft.OpenApi.Models;

namespace ExchangeRateUpdater.Host
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				// Set the comments path for the Swagger JSON and UI.
				foreach (var xmlFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
				{
					c.IncludeXmlComments(xmlFile);
				}

				var filePath = Path.Combine(System.AppContext.BaseDirectory, "ExchangeRateUpdater.API.xml");
				c.IncludeXmlComments(filePath);
			});

			builder.Services.Configure<ApiBehaviorOptions>
				(options => options.SuppressInferBindingSourcesForParameters = true);

			builder.Services.RegisterServices();

			var app = builder.Build();

			app.RegisterMiddlewares();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
