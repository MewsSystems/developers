using System;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure;

public sealed class CnbOptionsValidator : IValidateOptions<CnbOptions>
{
    public ValidateOptionsResult Validate(string? name, CnbOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Url))
        {
            return ValidateOptionsResult.Fail("Cnb:Url is required.");
        }

        if (!Uri.TryCreate(options.Url, UriKind.Absolute, out Uri? uri))
        {
            return ValidateOptionsResult.Fail("Cnb:Url must be a valid absolute URI.");
        }

        bool isHttpOrHttps =
            string.Equals(uri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);

        return isHttpOrHttps
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail("Cnb:Url must use http or https.");
    }
}
