using Mews.BackendDeveloperTask.ExchangeRates;
using Mews.BackendDeveloperTask.ExchangeRates.Cnb;

var colWidth = 30;

void WriteRow(params string[] columns)
{
    foreach (var column in columns)
    {
        Console.Write(column.PadRight(colWidth));
    }
    Console.WriteLine();
}

try
{
    var selectedCurrencies = new List<Currency>();
    foreach (var arg in args)
    {
        if (Enum.TryParse<Currency>(arg, out var currency))
        {
            selectedCurrencies.Add(currency);
        }
        else
        {
            Console.WriteLine($"Unrecognised currency code: {arg}");
        }
    }

    Console.WriteLine($"Retrieving exchange rates for currencies: {string.Join(", ", selectedCurrencies)}");

    var retriever = new CnbTextExchangeRateRetriever(new HttpClient());
    var parser = new CnbTextExchangeRateParser();
    var rates = await new CnbExchangeRateProvider(retriever, parser).GetExchangeRatesAsync(selectedCurrencies);

    WriteRow("Source", "Target", "Rate");
    Console.WriteLine(new string('=', colWidth * 3));

    if (!rates.Any())
    {
        Console.WriteLine("No exchange rates found");
    }

    foreach (var rate in rates)
    {
        WriteRow(rate.Source.ToString(), rate.Target.ToString(), rate.Rate.ToString());
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}