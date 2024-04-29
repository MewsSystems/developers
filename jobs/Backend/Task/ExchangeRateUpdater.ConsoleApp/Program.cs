using ExchangeRateUpdater.Domain;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main()
        {
            var currencies = new[]
{
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };

            using (var client = new HttpClient())
            {
                try
                {
                    const string uri = "https://localhost:5064/ExchangeRate";

                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();

                    var data = await response.Content.ReadAsStringAsync();

                    var rates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRate>>(data);

                    if (rates != null)
                    {
                        rates = rates.Where(x =>
                            currencies.Any(y => y.Code == x.SourceCurrency.Code) &&
                            currencies.Any(y => y.Code == x.TargetCurrency.Code));

                        var exchangeRates = rates.ToList();

                        Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");

                        foreach (var rate in exchangeRates)
                        {
                            Console.WriteLine(rate.ToString());
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }
}
