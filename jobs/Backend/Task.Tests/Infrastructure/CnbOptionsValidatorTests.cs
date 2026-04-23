using ExchangeRateUpdater.Infrastructure;
using Xunit;

namespace Task.Tests.Infrastructure;

public sealed class CnbOptionsValidatorTests
{
    private readonly CnbOptionsValidator _validator = new();

    [Fact]
    public void Validate_valid_http_url_returns_success()
    {
        CnbOptions options = new()
        {
            Url = "http://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/denni_kurz.txt"
        };

        var result = _validator.Validate(name: null, options);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_valid_https_url_returns_success()
    {
        CnbOptions options = new()
        {
            Url = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/denni_kurz.txt"
        };

        var result = _validator.Validate(name: null, options);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_relative_url_returns_failure()
    {
        CnbOptions options = new()
        {
            Url = "/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/denni_kurz.txt"
        };

        var result = _validator.Validate(name: null, options);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Validate_non_http_scheme_returns_failure()
    {
        CnbOptions options = new()
        {
            Url = "ftp://www.cnb.cz/path"
        };

        var result = _validator.Validate(name: null, options);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Validate_empty_url_returns_failure()
    {
        CnbOptions options = new()
        {
            Url = ""
        };

        var result = _validator.Validate(name: null, options);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Validate_malformed_url_returns_failure()
    {
        CnbOptions options = new()
        {
            Url = "http://"
        };

        var result = _validator.Validate(name: null, options);

        Assert.False(result.Succeeded);
    }
}
