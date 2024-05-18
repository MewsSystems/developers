using ExchangeRateUpdater.API;

public static class Program
{
    public static void Main(string[] args) 
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });
    }
}