namespace ExchangeRates.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication ConfigureApp(this WebApplication app, IHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rates API v1");
                c.RoutePrefix = "swagger";
            });

            app.MapControllers();
            app.Map("/error", (HttpContext httpContext) =>
            {
                return Results.Problem("An unexpected error occurred. Please try again later.");
            });

            return app;
        }
    }
}
