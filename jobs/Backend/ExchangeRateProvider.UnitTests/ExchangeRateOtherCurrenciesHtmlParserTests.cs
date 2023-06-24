using ExchangeRateProvider.Services;
using ExchangeRateProvider.Models;
using FluentAssertions;

namespace ExchangeRateProvider.UnitTests
{
    public class ExchangeRateOtherCurrenciesHtmlParserTests
    {
        [Fact]
        public void ExtractCurrencyExchangeRates_Should_Return_Parsed_Rates()
        {
            // arrange

            #region html

            var html = @"
                        <table>
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
                                 <td>Algeria</td>
                                 <td>dinar</td>
                                 <td align=""right"">100</td>
                                 <td align=""center"">DZD</td>
                                 <td align=""right"">16.251</td>
                              </tr>
                              <tr>
                                 <td>Angola</td>
                                 <td>kwanza</td>
                                 <td align=""right"">100</td>
                                 <td align=""center"">AOA</td>
                                 <td align=""right"">3.864</td>
                              </tr>
                              <tr>
                                 <td>Argentina</td>
                                 <td>peso</td>
                                 <td align=""right"">1</td>
                                 <td align=""center"">ARS</td>
                                 <td align=""right"">0.093</td>
                              </tr>
                           </tbody>
                        </table>
            ";
            
            #endregion

            var parser = new ExchangeRateOtherCurrenciesHtmlParser();
            var targetCurrency = "CZK";
            var expectedExchangeRates = new[]
            {
                new ExchangeRate(new Currency("DZD"), new Currency(targetCurrency), (decimal)0.16251),
                new ExchangeRate(new Currency("AOA"), new Currency(targetCurrency), (decimal)0.03864),
                new ExchangeRate(new Currency("ARS"), new Currency(targetCurrency), (decimal)0.093)
            };

            // act

            var actualExchangeRates = parser.ExtractCurrencyExchangeRates(targetCurrency, html);

            // assert
            actualExchangeRates.Should().BeEquivalentTo(expectedExchangeRates);
        }
    }
}
