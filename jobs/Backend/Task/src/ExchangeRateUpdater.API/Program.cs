using ExchangeRateUpdater.API.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Services.AddOpenApiAndSwagger();


WebApplication app = builder.Build();

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

public partial class Program
{
}