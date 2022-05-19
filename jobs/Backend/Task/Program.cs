using ExchangeRateUpdater;
using ExchangeRateUpdater.RatesReader;

using System;
using System.Linq;
using System.Net.Http;

try
{
    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(@"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
    var currenRatesReaderService = new AllCurrentRatesReaderService(httpClient);

    var rates = currenRatesReaderService.GetAllExchangeRates().Result;
    if(rates.Succsess)
    {
        Console.WriteLine($"Successfully retrieved {rates.Value.Count()} exchange rates:");
        foreach (var rate in rates.Value)
        {
            Console.WriteLine(rate.ToString());
        }
    }
    else
    {
        Console.WriteLine("There have bee some errors when retrieving the rates:");
        foreach(var reason in rates.FailureResons)
        {
            Console.WriteLine($"Reason: {reason}");
        }
    }
}
catch (Exception e)
{
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.ReadLine();