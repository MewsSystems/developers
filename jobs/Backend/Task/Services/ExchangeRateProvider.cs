using AutoMapper;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHtmlParserService htmlParserService;

        IMapper mapper { get; }

        public ExchangeRateProvider(IMapper mapper, IHtmlParserService htmlParserService)
        {
            this.mapper = mapper;
            this.htmlParserService = htmlParserService;
        }

        public async Task<List<CurrencyReadDTO>> GetExchangeRates()
        {
            var listOfCurrencies = await htmlParserService.GetDataFromSource();
            var retrievedCurrencies = StoringRetrievedData(listOfCurrencies, mapper);

            return retrievedCurrencies;
        }

        private List<CurrencyReadDTO> StoringRetrievedData(List<List<string>> listOfCurrencies, IMapper mapper)
        {
            List<CurrencyReadDTO> currencyListDTOs = new List<CurrencyReadDTO>();

            if (listOfCurrencies != null)
            {
                foreach (var item in listOfCurrencies)
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
    }
}
