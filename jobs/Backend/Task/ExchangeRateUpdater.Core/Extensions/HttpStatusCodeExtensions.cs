using System;
using System.Net;

namespace ExchangeRateUpdater.Core.Extensions;

internal static class HttpStatusCodeExtensions
{
	public static void ThrowIfNoSuccessful(this HttpStatusCode statusCode, string? errorMessage = null)
	{
		if (!statusCode.IsSuccess())
		{
			throw new InvalidOperationException(errorMessage ?? "The last operation failed");
		}
	}

	public static bool IsSuccess(this HttpStatusCode statusCode)
	{
		var code = (int)statusCode;

		return code is >= 200 and <= 299;
	}
}