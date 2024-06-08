using Application.Common.Models;
using Application.CzechNationalBank.ApiClient.Dtos;
using Application.CzechNationalBank.Mappings;

using FakeItEasy;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.CzechNationalBank.Mappings
{
    public class CNBExchangeRateMappingTests
    {
        private readonly ILogger<CNBExchangeRateMappingService> logger;
        private readonly CNBExchangeRateMappingService cnbExchangeRateMappingService;

        public CNBExchangeRateMappingTests()
        {
            logger = A.Fake<ILogger<CNBExchangeRateMappingService>>();
            cnbExchangeRateMappingService = new CNBExchangeRateMappingService(logger);
        }


        [Fact]
        public void ConvertToExchangeRates_WithValidInput_ReturnsExchangeRate()
        {
            var cnbDto = new CNBExRateDailyRestDto
            {
                CurrencyCode = "ZAR",
                Rate = 1.2m,
                Amount = 1
            };

            var result = cnbExchangeRateMappingService.ConvertToExchangeRates(new[] { cnbDto });

            result.Should().BeEquivalentTo(new List<ExchangeRate> { new ExchangeRate(new Currency("ZAR"), new Currency("CZK"), 1.2m) });
        }

        [Fact]
        public void ConvertToExchangeRates_WithAmount0_ReturnsEmpty()
        {
            var cnbDto = new CNBExRateDailyRestDto
            {
                CurrencyCode = "ZAR",
                Rate = 1.2m,
                Amount = 0
            };

            var result = cnbExchangeRateMappingService.ConvertToExchangeRates(new[] { cnbDto });

            result.Should().BeEmpty();
        }
    }
}
