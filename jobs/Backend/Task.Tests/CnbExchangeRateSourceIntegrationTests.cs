using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

public class CnbExchangeRateSourceIntegrationTests
{
    private const string TestUrl = "https://example.test/denni_kurz.txt";

    /// <summary>
    /// Matrix of CNB daily document shapes (as returned by HTTP) and expected parsed rates.
    /// Filtering by requested currencies happens in <see cref="ExchangeRateProvider"/>; the source returns all parsed rows.
    /// </summary>
    public static IEnumerable<object[]> CnbDocumentScenarios()
    {
        const string header = "07 May 2026 #87\nCountry|Currency|Amount|Code|Rate\n";

        yield return new object[]
        {
            "single_row_amount_one_dot_decimal",
            header + "USA|dollar|1|USD|20.648\n",
            new (string Src, string Tgt, decimal Value)[] { ("USD", "CZK", 20.648m) },
        };

        yield return new object[]
        {
            "amount_100_normalises_to_per_unit",
            header + "Japan|yen|100|JPY|13.204\n",
            new (string Src, string Tgt, decimal Value)[] { ("JPY", "CZK", 0.13204m) },
        };

        yield return new object[]
        {
            "multiple_rows",
            header
                + "USA|dollar|1|USD|20.648\n"
                + "EMU|euro|1|EUR|24.305\n",
            new (string Src, string Tgt, decimal Value)[]
            {
                ("USD", "CZK", 20.648m),
                ("EUR", "CZK", 24.305m),
            },
        };

        yield return new object[]
        {
            "comma_decimal_czech_style",
            header + "USA|dollar|1|USD|20,648\n",
            new (string Src, string Tgt, decimal Value)[] { ("USD", "CZK", 20.648m) },
        };

        yield return new object[]
        {
            "malformed_pipe_row_skipped_valid_rows_kept",
            header
                + "not|enough|columns\n"
                + "USA|dollar|1|USD|1\n"
                + "||||\n",
            new (string Src, string Tgt, decimal Value)[] { ("USD", "CZK", 1m) },
        };

        yield return new object[]
        {
            "empty_body",
            "",
            System.Array.Empty<(string Src, string Tgt, decimal Value)>(),
        };

        yield return new object[]
        {
            "headers_only_no_data_rows",
            header.TrimEnd(),
            System.Array.Empty<(string Src, string Tgt, decimal Value)>(),
        };

        yield return new object[]
        {
            "whitespace_trimmed_in_fields",
            header + "  USA  |  dollar  |  1  |  USD  |  10  \n",
            new (string Src, string Tgt, decimal Value)[] { ("USD", "CZK", 10m) },
        };
    }

    [Theory]
    [MemberData(nameof(CnbDocumentScenarios))]
    public async Task GetExchangeRates_parses_http_body_as_cnb_daily_document(
        string scenario,
        string httpBody,
        (string Src, string Tgt, decimal Value)[] expected)
    {
        _ = scenario;
        using var handler = new StubHttpMessageHandler(HttpStatusCode.OK, httpBody);
        using var httpClient = new HttpClient(handler);
        var options = Options.Create(new CnbOptions { DailyKurzUrl = TestUrl });
        var source = new CnbExchangeRateSource(httpClient, options, NullLogger<CnbExchangeRateSource>.Instance);

        var rates = (await source.GetExchangeRates()).ToList();

        Assert.Equal(expected.Length, rates.Count);
        foreach (var exp in expected)
        {
            Assert.Contains(
                rates,
                r => r.SourceCurrency.Code == exp.Src
                     && r.TargetCurrency.Code == exp.Tgt
                     && r.Value == exp.Value);
        }
    }

    [Fact]
    public async Task GetExchangeRates_when_http_fails_throws()
    {
        using var handler = new StubHttpMessageHandler(HttpStatusCode.NotFound, body: "");
        using var httpClient = new HttpClient(handler);
        var options = Options.Create(new CnbOptions { DailyKurzUrl = TestUrl });
        var source = new CnbExchangeRateSource(httpClient, options, NullLogger<CnbExchangeRateSource>.Instance);

        await Assert.ThrowsAsync<HttpRequestException>(
            () => source.GetExchangeRates());
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _body;

        public StubHttpMessageHandler(HttpStatusCode statusCode, string body)
        {
            _statusCode = statusCode;
            _body = body;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_body),
            };
            return Task.FromResult(response);
        }
    }
}
