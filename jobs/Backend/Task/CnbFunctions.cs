using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using static ExchangeRateUpdater.Types;
using static LanguageExt.Prelude;

namespace ExchangeRateUpdater
{
    public static class CnbFunctions
    {
        private const string Czk = "CZK";
        private const int HeaderRowCount = 2;
        
        public static Aff<string> LoadCnbFileContent(HttpClient httpClient, string url) =>
            Aff(async () => await httpClient.GetStringAsync(url));
        
        /// <summary>
        /// From supplied CNB file returns only lines with currency data - skips header rows.
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static Seq<string> GetCnbFileLines(string fileContent) =>
            (from content in Optional(fileContent)
                from a in Optional(content.Split("\n").Skip(HeaderRowCount))
                select toSeq(a))
            .Match(seq => seq,
                () => LanguageExt.Seq<string>.Empty);
        
        public static Seq<CnbCurrencyData> ParseCnbCurrencyItems(Seq<string> lines) =>
            lines
                .Select(FromCnbLine)
                .Filter(ccd => ccd.IsSome)
                .Sequence(ccd => ccd)
                .Match(seq => seq,
                    () => LanguageExt.Seq<CnbCurrencyData>.Empty);
        
        public static Option<CnbCurrencyData> FromCnbLine(string line) =>
            (from l in Optional(line).ToEff()
                from parts in Optional(toSeq(l.Split("|").TakeLast(3))).ToEff()
                from threeParts in (parts.Length() != 3
                        ? Option<Seq<string>>.None
                        : Option<Seq<string>>.Some(parts))
                    .ToEff()
                from currencyData in Eff(() => new CnbCurrencyData(
                    int.Parse(threeParts[0]),
                    new Currency(threeParts[1].Trim()),
                    decimal.Parse(threeParts[2])))
                select currencyData)
            .Run()
            .Match(Option<CnbCurrencyData>.Some,
                error => Option<CnbCurrencyData>.None);

        
        public static IEnumerable<ExchangeRate> ConvertCnbData(
            Seq<CnbCurrencyData> cnbCurrencyData,
            IEnumerable<Currency> requestedCurrencies) =>
            requestedCurrencies
                .Select(rc => TryFindExchangeRate(cnbCurrencyData, rc))
                .Filter(oe => oe.IsSome)
                .Sequence(oe => oe)
                .Match(
                    seq => seq,
                    () => LanguageExt.Seq<ExchangeRate>.Empty);

        public static Option<ExchangeRate> TryFindExchangeRate(Seq<CnbCurrencyData> cnbData, Currency targetCurrency) =>
            from cnbItem in 
                Optional(cnbData.FirstOrDefault(d => AreMatchingCurrencies(d, targetCurrency)))
            from exchangeRate in CreateExchangeRate(cnbItem, targetCurrency)
            select exchangeRate;

        /// <summary>
        /// Returns true if both parameters describe same currency.
        /// </summary>
        public static bool AreMatchingCurrencies(CnbCurrencyData cnbCurrencyData, Currency currency) =>
            cnbCurrencyData.Currency.Code == currency.Code;

        public static Option<ExchangeRate> CreateExchangeRate(CnbCurrencyData cnbCurrencyData, Currency currency) =>
             Eff(() => new ExchangeRate(currency,
                new Currency(Czk),
                cnbCurrencyData.CalculateRateToOneCzk()))
                 .Run()
                 .Match(er => er,
                     error => Option<ExchangeRate>.None);

        public static async Task<Seq<CnbCurrencyData>> DownloadCnbCurrencyData(
            Aff<string> getFileContent,
            Func<Error, Unit> errorHandler)
        {
            var computation = from fileContent in getFileContent
                from cnbLines in Eff(() => GetCnbFileLines(fileContent))
                from currencyData in Eff(() => ParseCnbCurrencyItems(cnbLines))
                select currencyData;

            var result = await computation.Run();

            return result.Match(
                seq => seq,
                error =>
                {
                    errorHandler(error);
                    return LanguageExt.Seq<CnbCurrencyData>.Empty;
                });
        }
    }
}