using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Tests.Mocks;

internal class ExchangeRateLoaderMock : IExchangeRateLoader
{
    private static string DATA => @"21.09.2022 #184
země|měna|množství|kód|kurz
Austrálie|dolar|1|AUD|16,588
Brazílie|real|1|BRL|4,838
Bulharsko|lev|1|BGN|12,596
Čína|žen-min-pi|1|CNY|3,529
Dánsko|koruna|1|DKK|3,313
EMU|euro|1|EUR|24,635
Filipíny|peso|100|PHP|43,006
Hongkong|dolar|1|HKD|3,168
Chorvatsko|kuna|1|HRK|3,275
Indie|rupie|100|INR|31,121
Indonesie|rupie|1000|IDR|1,658
Island|koruna|100|ISK|17,559
Izrael|nový šekel|1|ILS|7,180
Japonsko|jen|100|JPY|17,273
Jižní Afrika|rand|1|ZAR|1,408
Kanada|dolar|1|CAD|18,576
Korejská republika|won|100|KRW|1,783
Maďarsko|forint|100|HUF|6,082
Malajsie|ringgit|1|MYR|5,463
Mexiko|peso|1|MXN|1,245
MMF|ZPČ|1|XDR|32,207
Norsko|koruna|1|NOK|2,395
Nový Zéland|dolar|1|NZD|14,626
Polsko|zlotý|1|PLN|5,186
Rumunsko|leu|1|RON|4,982
Singapur|dolar|1|SGD|17,590
Švédsko|koruna|1|SEK|2,256
Švýcarsko|frank|1|CHF|25,800
Thajsko|baht|100|THB|66,977
Turecko|lira|1|TRY|1,357
USA|dolar|1|USD|24,870
Velká Británie|libra|1|GBP|28,210";
    
    public async Task<Stream> ReadAsync()
    {
        var ms = new MemoryStream();
        var sw = new StreamWriter(ms);
        await sw.WriteAsync(DATA);

        return ms;
    }
}