using System;
using System.Globalization;
using System.Collections.Generic;
using ExchangeRateUpdater.Utilities.Extensions;
using ExchangeRateUpdater.Utilities.StructureMap;
using ExchangeRateUpdater.Utilities.AutoMapper;
using ExchangeRateUpdater.ViewModels;

namespace ExchangeRateUpdater.Mappings
{
    public class CnbExRatesToExRatesMapping : IRegisterAutoMapper
    {
        private readonly AutoMapperConfigurationProfile _profile;
        
        public CnbExRatesToExRatesMapping(AutoMapperConfigurationProfile profile)
        {
            _profile = profile;
        }
        
        public void Register()
        {
            _profile.CreateMap<kurzy, IEnumerable<ExchangeRate>>().ConvertUsing(source => converter(source));
        }

        private static IEnumerable<ExchangeRate> converter(kurzy source)
        {
            var rates = new List<ExchangeRate>();

            source?.tabulka.ForEach(kurzyTabulka =>
                kurzyTabulka?.radek.ForEach(kurzyTabulkaRadek =>
                    rates.Add(new ExchangeRate(
                        new Currency(kurzyTabulkaRadek.kod), decimal.Parse(kurzyTabulkaRadek.kurz, CultureInfo.CurrentCulture)))));

            return rates;
        }
    }
}
