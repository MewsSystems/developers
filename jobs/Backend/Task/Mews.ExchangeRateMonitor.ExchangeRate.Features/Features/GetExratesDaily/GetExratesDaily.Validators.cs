using FluentValidation;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Options;
using Microsoft.Extensions.Options;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;

public class GetExratesDailyRequestValidator : AbstractValidator<GetExratesDailyRequest>
{
    public GetExratesDailyRequestValidator(IOptions<ExchangeRateModuleOptions> opts)
    {
        RuleFor(req => req.Date)
            .NotNull().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date cannot be in the future.");
    }
}
