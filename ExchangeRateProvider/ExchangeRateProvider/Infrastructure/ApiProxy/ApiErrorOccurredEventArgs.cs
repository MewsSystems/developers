using System;
using System.Web.Http;

namespace ExchangeRateProvider.Infrastructure.ApiProxy
{
    /// <summary>
    /// ApiErrorOccurredEventArgs decorates HTTP error code
    /// </summary>
    public class ApiErrorOccurredEventArgs : EventArgs
    {
        public ApiErrorOccurredEventArgs(HttpError error)
        {
            Error = error;
        }

        public HttpError Error { get; }
    }
}