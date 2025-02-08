using System.Net;
using FuncSharp;

namespace ExchangeRateUpdater.Lib.Exception;

public static class ExceptionExtensions
{
    private static readonly HashSet<WebExceptionStatus> ConnectionErrorStatuses =
    [
        WebExceptionStatus.ConnectFailure,
        WebExceptionStatus.ConnectionClosed,
        WebExceptionStatus.SecureChannelFailure,
        WebExceptionStatus.NameResolutionFailure
    ];

    public static bool IsServerError(this WebException @this) => @this.Response
        .As<HttpWebResponse>()
        .Map(r => (int)r.StatusCode)
        .Map(code => code is > 500 and < 599)
        .GetOrFalse();

    public static bool IsConnectionError(this WebException @this) => ConnectionErrorStatuses.Contains(@this.Status);
}