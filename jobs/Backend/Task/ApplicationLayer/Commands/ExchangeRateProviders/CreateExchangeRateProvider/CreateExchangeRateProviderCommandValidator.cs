using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;

/// <summary>
/// Validator for CreateExchangeRateProviderCommand.
/// </summary>
public class CreateExchangeRateProviderCommandValidator : AbstractValidator<CreateExchangeRateProviderCommand>
{
    public CreateExchangeRateProviderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Provider name is required.")
            .MaximumLength(200).WithMessage("Provider name must not exceed 200 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Provider code is required.")
            .MaximumLength(10).WithMessage("Provider code must not exceed 10 characters.")
            .Matches("^[A-Z0-9_]+$").WithMessage("Provider code must contain only uppercase letters, numbers, and underscores.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Provider URL is required.")
            .Must(BeAValidUrl).WithMessage("Provider URL must be a valid HTTP or HTTPS URL.");

        RuleFor(x => x.BaseCurrencyId)
            .GreaterThan(0).WithMessage("Base currency ID must be positive.");

        RuleFor(x => x.ApiKeyVaultReference)
            .NotEmpty().WithMessage("API key vault reference is required when authentication is enabled.")
            .When(x => x.RequiresAuthentication);
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
