namespace ExchangeRateDemo.Infrastructure.Providers
{
    public interface IRestProvider
    {
        HttpClient Client { get; set; }
    }
}
