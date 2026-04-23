using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateProvider
    {
        private const string CnbDailyUrl =
            "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

        private static readonly StringComparer CodeComparer = StringComparer.OrdinalIgnoreCase;

        private readonly HttpClient _http;
        private readonly ILogger<ExchangeRateProvider> _log;

        // Reuse also prevents pointless allocations.
        private readonly ConcurrentDictionary<string, Currency> _currencyCache = new(CodeComparer);

        public ExchangeRateProvider(HttpClient http, ILogger<ExchangeRateProvider> log)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _log = log ?? throw new ArgumentNullException(nameof(log));

            // If someone forgot to configure it, keep us from hanging forever.
            if (_http.Timeout == Timeout.InfiniteTimeSpan)
                _http.Timeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Returns CZK->X exchange rates for requested currencies.
        /// </summary>
        public async Task<IReadOnlyList<ExchangeRate>> GetCzkBasedRatesAsync(
            IEnumerable<Currency> currencies,
            CancellationToken ct = default)
        {
            if (currencies is null) throw new ArgumentNullException(nameof(currencies));

            var requested = new HashSet<string>(
                currencies.Select(c => c?.Code).Where(s => !string.IsNullOrWhiteSpace(s))!,
                CodeComparer);

            if (requested.Count == 0)
                return Array.Empty<ExchangeRate>();

            // CNB daily.txt is CZK-based. If caller doesn't care about CZK, we can't help here.
            if (!requested.Contains("CZK"))
                return Array.Empty<ExchangeRate>();

            string body;
            try
            {
                body = await DownloadWithRetryAsync(CnbDailyUrl, ct).ConfigureAwait(false);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                _log.LogError(ex, "Failed to download CNB daily rates.");
                throw;
            }

            var all = ParseCnbDailyTxt(body);

            // Return only CZK->requested (excluding CZK->CZK)
            var result = all
                .Where(r => requested.Contains(r.TargetCurrency.Code) &&
                            !CodeComparer.Equals(r.TargetCurrency.Code, "CZK"))
                .ToList();

            _log.LogDebug("CNB rates: parsed={Parsed}, returned={Returned}", all.Count, result.Count);
            return result;
        }

        private async Task<string> DownloadWithRetryAsync(string url, CancellationToken ct)
        {
            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using var req = new HttpRequestMessage(HttpMethod.Get, url);

                    using var resp = await _http.SendAsync(
                            req,
                            HttpCompletionOption.ResponseHeadersRead,
                            ct)
                        .ConfigureAwait(false);

                    if (IsTransient(resp.StatusCode) && attempt < maxAttempts)
                    {
                        var delay = Backoff(attempt);
                        _log.LogWarning("CNB request transient ({StatusCode}). Attempt {Attempt}/{Max}. Retrying in {Delay}ms.",
                            (int)resp.StatusCode, attempt, maxAttempts, (int)delay.TotalMilliseconds);

                        await Task.Delay(delay, ct).ConfigureAwait(false);
                        continue;
                    }

                    if (!resp.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(
                            $"CNB request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}");
                    }


                    return await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    // Caller canceled: don’t retry, just bubble.
                    throw;
                }
                catch (TaskCanceledException ex) when (attempt < maxAttempts)
                {
                    // Timeout typically ends up here; treat as transient.
                    var delay = Backoff(attempt);
                    _log.LogWarning(ex, "CNB request timed out. Attempt {Attempt}/{Max}. Retrying in {Delay}ms.",
                        attempt, maxAttempts, (int)delay.TotalMilliseconds);

                    await Task.Delay(delay, ct).ConfigureAwait(false);
                }
                catch (HttpRequestException ex) when (attempt < maxAttempts)
                {
                    var delay = Backoff(attempt);
                    _log.LogWarning(ex, "CNB request failed (network). Attempt {Attempt}/{Max}. Retrying in {Delay}ms.",
                        attempt, maxAttempts, (int)delay.TotalMilliseconds);

                    await Task.Delay(delay, ct).ConfigureAwait(false);
                }
            }

            throw new InvalidOperationException("Failed to download CNB exchange rates after multiple attempts.");
        }

        private static bool IsTransient(HttpStatusCode code)
        {
            var n = (int)code;
            return code == HttpStatusCode.TooManyRequests || (n >= 500 && n <= 599);
        }

        private static TimeSpan Backoff(int attempt)
            => TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt - 1));


        private List<ExchangeRate> ParseCnbDailyTxt(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new FormatException("Unexpected CNB response: empty body.");

            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // CNB daily.txt typically has 2 header lines, then data rows.
            if (lines.Length < 3)
                throw new FormatException($"Unexpected CNB response format. Lines={lines.Length}.");

            var czk = GetCurrency("CZK");
            var result = new List<ExchangeRate>(capacity: Math.Max(0, lines.Length - 2));

            for (int i = 2; i < lines.Length; i++)
            {
                // Expected: Country|Currency|Amount|Code|Rate
                var parts = lines[i].Split('|');
                if (parts.Length != 5)
                {
                    _log.LogDebug("Skipping CNB line {Line}: unexpected field count ({Count}).", i + 1, parts.Length);
                    continue;
                }

                if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount) || amount <= 0)
                {
                    _log.LogDebug("Skipping CNB line {Line}: bad amount '{Amount}'.", i + 1, parts[2]);
                    continue;
                }

                var code = (parts[3] ?? "").Trim();
                if (code.Length != 3)
                {
                    _log.LogDebug("Skipping CNB line {Line}: bad code '{Code}'.", i + 1, code);
                    continue;
                }

                if (!decimal.TryParse(parts[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var rate) || rate <= 0m)
                {
                    _log.LogDebug("Skipping CNB line {Line}: bad rate '{Rate}'.", i + 1, parts[4]);
                    continue;
                }

                var target = GetCurrency(code);

                // CNB provides CZK per (amount) units => normalize to CZK per 1 unit
                result.Add(new ExchangeRate(
                    sourceCurrency: czk,
                    targetCurrency: target,
                    value: rate / amount));
            }

            if (result.Count == 0)
                _log.LogWarning("CNB response parsed successfully but produced 0 rates. Header: '{Header}'", lines[0]);

            return result;
        }

        private Currency GetCurrency(string code)
            => _currencyCache.GetOrAdd(code.Trim(), c => new Currency(c.Trim().ToUpperInvariant()));
    }
}
