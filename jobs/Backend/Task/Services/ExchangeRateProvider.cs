using AutoMapper;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly string URL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/";
        IMapper mapper { get; }

        public ExchangeRateProvider(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<List<CurrencyReadDTO>> GetExchangeRates()
        {
            var listOfCurrencies = await GetDataFromSource();
            var retrievedCurrencies = StoringRetrievedData(listOfCurrencies, mapper);

            return retrievedCurrencies;
        }

        private async Task<List<List<string>>> GetDataFromSource()
        {
            using (var client = new HttpClient())
            {
                var html = await client.GetStringAsync(URL);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var currencyData = doc.DocumentNode.SelectSingleNode("//tbody");
                var listOfCurrencies = currencyData.Descendants("tr").Select(tr => tr.Descendants("td")
                                        .Select(td => WebUtility.HtmlDecode(td.InnerText)).ToList()).ToList();

                if(listOfCurrencies != null || listOfCurrencies.Any())
                {
                    return listOfCurrencies;
                }
                else
                {
                    return null;
                }
            }
        }

        private List<CurrencyReadDTO> StoringRetrievedData(List<List<string>> listOfCurrencies, IMapper mapper)
        {
            List<CurrencyReadDTO> currencyListDTOs = new List<CurrencyReadDTO>();

            if(listOfCurrencies != null)
            {
                foreach (var item in listOfCurrencies)
                {
                    if (item.Count == 0)
                    {
                        continue;
                    }

                    else
                    {
                        Currency currency = new Currency();
                        foreach (var value in item)
                        {
                            currency.Country = item[0];
                            currency.CurrencyName = item[1];
                            currency.Amount = item[2];
                            currency.Code = item[3];
                            currency.Rate = item[4];
                        }

                        var currencyDTO = mapper.Map<CurrencyReadDTO>(currency);
                        currencyListDTOs.Add(currencyDTO);
                    }
                }
                return currencyListDTOs;
            }
            else
            {
                return null;
            }
            
        }
    }
}
