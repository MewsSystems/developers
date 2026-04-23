namespace ExchangeRateDemo.Infrastructure.Providers
{
    public abstract class RestProvider : IRestProvider
    {
        public abstract string Name { get; }

        public required HttpClient Client { get; set; }
    }
}
