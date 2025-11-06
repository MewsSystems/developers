using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;

public class UpdateProviderConfigurationCommandValidator : AbstractValidator<UpdateProviderConfigurationCommand>
{
    public UpdateProviderConfigurationCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.Name != null)
            .WithMessage("Provider name must not exceed 200 characters.");

        RuleFor(x => x.Url)
            .NotEmpty()
            .MaximumLength(500)
            .Must(BeAValidUrl)
            .When(x => x.Url != null)
            .WithMessage("Provider URL must be a valid URL.");

        RuleFor(x => x.ApiKeyVaultReference)
            .NotEmpty()
            .MaximumLength(500)
            .When(x => x.RequiresAuthentication == true)
            .WithMessage("API key vault reference is required when authentication is enabled.");

        RuleFor(x => x)
            .Must(x => x.Name != null || x.Url != null || x.RequiresAuthentication != null)
            .WithMessage("At least one field must be provided for update.");
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
