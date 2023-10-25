using System.Collections.Concurrent;
using ExchangeRateUpdate.Testing;
using NUnit.Framework;

namespace ExchangeRateUpdater.Testing;

[Parallelizable(ParallelScope.All)]
[Timeout(20 * 100)]
[TestFixture]
public class TestFixture<TContext, TSut>
	where TSut : class
	where TContext : TestContext<TSut>, new()
{
	private readonly ConcurrentDictionary<string, TContext> _contextDictionary = new();

	protected TContext Context
	{
		get { return _contextDictionary.GetOrAdd(GetIsolationContextId(), _ => new TContext()); }
	}

	private string GetIsolationContextId() => $"{TestContext.CurrentContext.Test.ID}";
}