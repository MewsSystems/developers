using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeRateUpdater;
using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Interfaces;
using FluentAssertions;
using NSubstitute;
using ToolsProvider.Helpers;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTest
    {
        private readonly IHtmlParserService htmlParserService = Substitute.For<IHtmlParserService>();
        private readonly IExchangeRateProvider sut;
        private readonly ExchangeRateProvider rateProvider;

        public ExchangeRateProviderTest()
        {
            var mapper = MappingProvider.GetMapper();
            sut = new ExchangeRateProvider(mapper, htmlParserService);
            rateProvider = new ExchangeRateProvider(mapper, new HtmlParserService());
        }


        [Fact]
        public async void Check_If_GetExchangeRates_Returns_Any_Data()
        {
            // Act
            var rates = rateProvider.GetExchangeRates();

            // Assert
            Assert.NotEmpty(rates.Result);
        }

        [Fact]
        public async Task Check_Correctness_DataReading()
        {
            // Arrange

            var moqData = new List<List<string>>
            {
                new List<string>
                {
                    "Australia",
                    "dollar",
                    "1",
                    "AUD",
                    "14.01"
                }
            };

            htmlParserService.GetDataFromSource().Returns(moqData);

            // Act
            var result = await sut.GetExchangeRates();

            // Assert
            var currencyReadDto = new CurrencyReadDTO
            {
                Country = moqData[0][0],
                CurrencyName = moqData[0][1],
                Amount = moqData[0][2],
                Code = moqData[0][3],
                Rate = moqData[0][4]
            };

            result.Count.Should().Be(1);
            result[0].Should().BeEquivalentTo(currencyReadDto);
        }
    }
}