using Common.Interface;

namespace Common.Exceptions
{
	/// <summary>
	/// Exception raised if a provider was not correctly built.
	/// </summary>
	/// <typeparam name="T">Provider type.</typeparam>
	public class ProviderNotBuiltException<T> : Exception where T : IExchangeRateProvider
	{
		public override string Message => $"Could not instantiate {typeof(T).Name} provider.";
	}

	/// <summary>
	/// Exception is raised in case of error in requesting exchange rates.
	/// </summary>
	public class GetExcangeRateException : Exception
	{
		public override string Message => $"Exception in getting exchange rates.";

		public GetExcangeRateException(Exception ex) : base(null, ex) { }
	}

	/// <summary>
	/// Exception is thrown if incorrect 3letter currency code was passed into Currency class constructor.
	/// </summary>
	public class IncorrectCurrencyCodeFormartException : Exception
	{
		public override string Message => $"Currency code symbol was Three-letter ISO 4217 code format.";
	}

	/// <summary>
	/// Exception is thrown if incorrect 3letter currency code was passed into Currency class constructor.
	/// </summary>
	public class LogInitializationException : Exception
	{
		public override string Message => $"Could not initialize log mechanism. Logging functionality disabled.";
	}
}
