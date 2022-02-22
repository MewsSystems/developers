using System.Xml.Serialization;
using ExchangeRateUpdater.DataAccess.Models;

namespace ExchangeRateUpdater.DataAccess;

public class ExchangeRateRepository
{
    private const string RATES_URL = "cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
    private readonly HttpClient _httpClient;
    

    public ExchangeRateRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, Radek>> GetRatesAsync()
    {
        var response = await _httpClient.GetAsync(RATES_URL);
        var xmlSerializer = new XmlSerializer(typeof(Kurzy));

        using (var responseContent = await response.Content.ReadAsStreamAsync())
        {
            var kurzy = (Kurzy) xmlSerializer.Deserialize(responseContent);

            if (kurzy.Tabulka == null)
            {
                throw new Exception("Response does not contain table with rates");
            }

            return kurzy.Tabulka.Radek?.ToDictionary(x => x.Kod, x => x) ?? new Dictionary<string, Radek>();
        }

        throw new Exception("Problem during getting rates");
    }
}