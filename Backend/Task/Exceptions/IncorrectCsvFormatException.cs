using System;

namespace ExchangeRateUpdater.Exceptions
{
	public class IncorrectCsvFormatException : Exception
	{
		public IncorrectCsvFormatException(string expectedHeader, string currentHeader)
		{
			Message = $"Incorrect csv format. Expected header was: {expectedHeader} but current header is: {currentHeader}.";
		}

		public override string Message { get; }
	}
}
