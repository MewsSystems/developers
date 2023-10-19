using Mews.ExchangeRateProvider.Exceptions;
using Polly;
using Polly.Registry;

namespace Mews.ExchangeRateProvider;

/// <summary>
/// Decorates calls to an <see cref="IExchangeRateProvider"/> with a Polly Policy
/// </summary>
public sealed class ExchangeRateProviderPolicyDecorator : IExchangeRateProvider
{
    private readonly IExchangeRateProvider _inner;
    private readonly ResiliencePipeline _pollyPipeline;

    internal const string PollyPolicyName = nameof(ExchangeRateProviderPolicyDecorator);

    /// <summary>
    /// Decorates calls to an <see cref="IExchangeRateProvider"/> with a Polly Policy
    /// </summary>
    /// <param name="inner">The Exchange Rate Provider instance to be wrapped in a policy</param>
    /// <param name="pollyProvider">Provides a Polly resiliency policy for the class</param>
    public ExchangeRateProviderPolicyDecorator(IExchangeRateProvider inner, ResiliencePipelineProvider<string> pollyProvider)
    {
        _inner = inner;
        _pollyPipeline = pollyProvider.GetPipeline(PollyPolicyName);
    }

    /// <summary>
    /// Obtains the most recent available exchange rate data for a given list of currencies from an inner provider, wrapped in a Polly policy
    /// </summary>
    /// <param name="currencies">A list of currency pairs to obtain exchange rate data for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A Task that will return a collection of <see cref="ExchangeRate"/> objects for each supplied currency, if known to the remote provider</returns>
    /// <exception cref="ArgumentNullException">Thrown if input parameters are null</exception>
    /// <exception cref="ArgumentException">Thrown if list of currencies is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown if no endpoint URIs are defined in the _options field</exception>
    /// <exception cref="ObtainExchangeRateException">Thrown if there is a issue obtaining exchange rate data</exception>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken) =>
        await _pollyPipeline.ExecuteAsync(async ct => await _inner.GetExchangeRatesAsync(currencies, ct), cancellationToken);
}