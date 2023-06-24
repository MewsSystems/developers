using ExchangeRateProvider.Services;
using ExchangeRateProvider.Models;
using FluentAssertions;

namespace ExchangeRateProvider.UnitTests
{
    public class ExchangeRateFixingHtmlParserTests
    {
        [Fact]
        public void ExtractCurrencyExchangeRates_Should_Return_Parsed_Rates()
        {
            // arrange

            #region html

            var html = @"
                        <table class=""currency-table"">
                           <thead>
                              <tr>
                                 <th>Country</th>
                                 <th>Currency</th>
                                 <th>Amount</th>
                                 <th>Code</th>
                                 <th>Rate</th>
                              </tr>
                           </thead>
                           <tbody>
                              <tr>
                                <td>Denmark</td>
                                <td>krone</td>
                                <td align=""right"">1</td>
                                <td align=""center"">DKK</td>
                                <td align=""right"">3.177</td>
                             </tr>
                             <tr>
                                    <td>EMU</td>
                                    <td>euro</td>
                                    <td align=""right"">1</td>
                                    <td align=""center"">EUR</td>
                                    <td align=""right"">23.660</td>
                             </tr>                        
                             <tr>
                                <td>Hungary</td>
                                <td>forint</td>   
                                <td align=""right"">100</td>
                                <td align=""center"">HUF</td>
                                <td align=""right"">6.397</td>
                             </tr>
                           </tbody>
                        </table>
            ";
            
            #endregion

            var parser = new ExchangeRateFixingHtmlParser();
            var targetCurrency = "CZK";
            var expectedExchangeRates = new[]
            {
                new ExchangeRate(new Currency("DKK"), new Currency(targetCurrency), (decimal)3.177),
                new ExchangeRate(new Currency("EUR"), new Currency(targetCurrency), (decimal)23.660),
                new ExchangeRate(new Currency("HUF"), new Currency(targetCurrency), (decimal)0.06397)
            };

            // act

            var actualExchangeRates = parser.ExtractCurrencyExchangeRates(targetCurrency, html);

            // assert
            actualExchangeRates.Should().BeEquivalentTo(expectedExchangeRates);
        }
    }
}
