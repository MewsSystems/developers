using System;
using System.Net;

namespace ExchangeRateUpdater.Domain.Core.Exceptions
{
	/// <summary>
	/// Domain exception class for managed exceptions in the service
	/// </summary>
	[Serializable]
	public class DomainException : Exception
	{
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
