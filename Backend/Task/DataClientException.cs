using System;

namespace ExchangeRateUpdater
{
	public class DataClientException: Exception
	{
		public DataClientException(string message) : base(message) { }

		public DataClientException() : base() { }

		public DataClientException(string message, Exception innerException) : base(message, innerException) { }
	}
}
