using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExchangeRateProvider.Infrastructure.HttpHelper
{
    /// <summary>
    /// Async HttpHelper with overriden issue requesr method
    /// </summary>
    public class HttpHelperAsync: HttpHelper
    {
        /// <summary>
        /// Issue non-blocking request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="completeAction"></param>
        /// <param name="failAction"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void IssueRequest<TResult>(Func<HttpClient, Task<HttpResponseMessage>> request, Action<TResult> completeAction,
            Action<HttpError> failAction, CancellationToken cancellationToken)
        {
            var client =  HttpHelper.CreateClient(TimeSpan.FromSeconds(15));

            request(client).ContinueWith(async httpResponseMessage =>
            {
                try
                {
                    var result = await httpResponseMessage;

                    if (result.IsSuccessStatusCode)
                    {
                        var resultContent = await result.Content.ReadAsAsync<TResult>(CancellationToken.None).ConfigureAwait(false);

                        completeAction?.Invoke(resultContent);
                        return;
                    }

                    failAction?.Invoke(await result.Content.ReadAsAsync<HttpError>().ConfigureAwait(false));
                }
                catch (AggregateException ex)
                {
                    failAction?.Invoke(new HttpError(ex.InnerExceptions[0], true));
                }
                // ReSharper disable once CatchAllClause
                catch (Exception ex)
                {
                    failAction?.Invoke(new HttpError(ex, true));
                }

            },
                cancellationToken,
                TaskContinuationOptions.PreferFairness |
                TaskContinuationOptions.LongRunning |
                TaskContinuationOptions.LazyCancellation |
                TaskContinuationOptions.HideScheduler,
                TaskScheduler.Default);

        }
    }
}