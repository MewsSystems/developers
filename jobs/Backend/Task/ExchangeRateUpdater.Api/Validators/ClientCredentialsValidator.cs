using ExchangeRateUpdater.Api.Models;
using FluentValidation;

namespace ExchangeRateUpdater.Api.Validators;

public class ClientCredentialsValidator : AbstractValidator<ClientCredentials>
{
    public ClientCredentialsValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required");

        RuleFor(x => x.ClientSecret)
            .NotEmpty()
            .WithMessage("Client Secret is required");
    }
} 