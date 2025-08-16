using Mews.ExchangeRate.Updater.Bootstrapper;
using Mews.ExchangeRate.Http.Cnb;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.Configure<ExchangeRateServiceClientOptions>(builder.Configuration.GetSection(ExchangeRateServiceClientOptions.ConfigurationSectionName));
        builder.Services.AddCommonServices();

        var app = builder.Build();

        // [JUAN] At this point, the host does not implement any API controllers,
        // but it is possible to add them later to manage Update on demand.
        
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