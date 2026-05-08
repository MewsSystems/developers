using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Mews.ExchangeRateMonitor.Common.Domain.Results;
using Mews.ExchangeRateMonitor.ExchangeRate.Domain;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Features;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Tests.Unit.Features.GetExratesDaily
{
    public class GetExratesDailyHandlerTests
    {
        [Fact]
        public async Task Returns_Errors_When_DateValidation_Fails()
        {
            var logger = Substitute.For<ILogger<GetExratesDailyHandler>>();
            var provider = Substitute.For<IExchangeRateProvider>();
            var validator = Substitute.For<IValidator<GetExratesDailyRequest>>();
            validator.ValidateAsync(Arg.Any<GetExratesDailyRequest>(), Arg.Any<CancellationToken>())
                     .Returns(new ValidationResult([new ValidationFailure("Date", "invalid")]));
            var sut = new GetExratesDailyHandler(logger, provider, validator);

            var res = await sut.HandleAsync(new GetExratesDailyRequest(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))), default);
            res.IsError.Should().BeTrue();
            await provider.DidNotReceiveWithAnyArgs().GetDailyRatesAsync(default, default);
        }

        [Fact]
        public async Task Bubbles_Provider_Errors()
        {
            var logger = Substitute.For<ILogger<GetExratesDailyHandler>>();
            var provider = Substitute.For<IExchangeRateProvider>();
            var validator = Substitute.For<IValidator<GetExratesDailyRequest>>();
            validator.ValidateAsync(Arg.Any<GetExratesDailyRequest>(), Arg.Any<CancellationToken>())
                     .Returns(new ValidationResult());
            provider.GetDailyRatesAsync(Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
                    .Returns(Error.Failure("Provider", "CNB down"));

            var sut = new GetExratesDailyHandler(logger, provider, validator);

            var res = await sut.HandleAsync(new GetExratesDailyRequest(new DateOnly(2025, 8, 22)), default);

            res.IsError.Should().BeTrue();
            res.Errors.Should().ContainSingle(e => e.Description.Contains("CNB down"));
        }

        [Fact]
        public async Task Maps_Domain_Rates_To_Dtos_On_Success()
        {
            var logger = Substitute.For<ILogger<GetExratesDailyHandler>>();
            var provider = Substitute.For<IExchangeRateProvider>();
            var validator = Substitute.For<IValidator<GetExratesDailyRequest>>();
            validator.ValidateAsync(Arg.Any<GetExratesDailyRequest>(), Arg.Any<CancellationToken>())
                     .Returns(new ValidationResult());
            provider.GetDailyRatesAsync(Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
                    .Returns(new List<CurrencyExchangeRate>
                    {
                        new(new("USD"),new("CZK"),1m, 22m),
                        new(new("EUR"),new("CZK"),1m, 26m)
                    });

            var sut = new GetExratesDailyHandler(logger, provider, validator);

            var res = await sut.HandleAsync(new GetExratesDailyRequest(new DateOnly(2025, 8, 22)), default);

            res.IsError.Should().BeFalse();
            res.Value!.Select(x => x.SourceCurrency).Should().BeEquivalentTo("USD", "EUR");
            res.Value!.All(x => x.TargetCurrency == "CZK").Should().BeTrue();
        }
    }
}
