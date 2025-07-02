using System;
using System.Net;

namespace ExchangeRateUpdater.Core.Exceptions
{
	/// <summary>
	/// Domain exception class for managed exceptions in the service.
	/// </summary>
	[Serializable]
	public class DomainException : Exception
	{
		/// <summary>
		/// Status code to send to the client.
		/// </summary>
		public HttpStatusCode StatusCode { get; }

		public DomainException(HttpStatusCode statusCode, string message) : base(message) 
		{
			this.StatusCode = statusCode;
		}

		public DomainException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
		{
			this.StatusCode = statusCode;
		}
	}
}
