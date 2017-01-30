using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExchangeRateProvider.Infrastructure.HttpHelper
{
    /// <summary>
    /// HttpHelper - Api Client Implementation
    /// </summary>
  public class HttpHelper : IHttpHelper
  {
    /// <summary>
    /// Creates a <see cref="HttpClient" /> with the default configuration.
    /// </summary>
    /// <returns></returns>
    private static HttpClient CreateClient()
    {
      var handler = new HttpClientHandler { UseDefaultCredentials = true };
      var client = new HttpClient(handler);
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      return client;
    }

    protected static HttpClient CreateClient(TimeSpan timeout)
    {
      var handler = new HttpClientHandler { UseDefaultCredentials = true };

      var client = new HttpClient(handler);

      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.Timeout = timeout;

      return client;
    }
    /// <summary>
    /// Issues a request to the corresponding service and handles the response.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="request">The request.</param>
    /// <param name="completeAction">An action to invoke when the request completes.</param>
    /// <param name="failAction">An action to invoke when the request fails.</param>
    private static void IssueRequest<TResult>(Func<HttpClient, Task<HttpResponseMessage>> request, Action<TResult> completeAction, Action<HttpError> failAction)
    {
      var client = CreateClient();
      request(client).ContinueWith(async httpResponseMessage =>
      {
        try
        {
          var result = await httpResponseMessage;

          if (result.IsSuccessStatusCode)
          {
            var resultContent = await result.Content.ReadAsAsync<TResult>();
            completeAction?.Invoke(resultContent);
            return;
          }

          failAction?.Invoke(await result.Content.ReadAsAsync<HttpError>());
        }
        catch (AggregateException ex)
        {
         failAction?.Invoke(new HttpError(ex.InnerExceptions[0], true));
        }
        catch (Exception ex)
        {
         failAction?.Invoke(new HttpError(ex, true));
        }

      }, TaskContinuationOptions.PreferFairness |
         TaskContinuationOptions.LongRunning |
         TaskContinuationOptions.LazyCancellation |
         TaskContinuationOptions.HideScheduler);
    }

    /// <summary>
    /// Issues a request (GET) for retrieving one or more objects.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="address">The address.</param>
    /// <param name="completeAction">An action to invoke when the request completes.</param>
    /// <param name="failAction">An action to invoke when the request fails.</param>
    public void Get<TResult>(string address, Action<TResult> completeAction, Action<HttpError> failAction)
    {
      IssueRequest(c => c.GetAsync(address), completeAction, failAction);
    }

    /// <summary>
    /// Issues a request (PUT) for updating an object.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="address">The address.</param>
    /// <param name="getPayload">The get payload.</param>
    /// <param name="completeAction">An action to invoke when the request completes.</param>
    /// <param name="failAction">An action to invoke when the request fails.</param>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public void Put<TResult>(string address, Func<HttpContent> getPayload, Action<TResult> completeAction, Action<HttpError> failAction)
    {
      IssueRequest(
          c => c.PutAsync(address, getPayload()),
          completeAction,
          failAction
      );
    }

    /// <summary>
    /// Issues a request (POST) for creating an object.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="address">The address.</param>
    /// <param name="getPayload">The get payload.</param>
    /// <param name="completeAction">An action to invoke when the request completes.</param>
    /// <param name="failAction">An action to invoke when the request fails.</param>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public void Post<TResult>(string address, Func<HttpContent> getPayload, Action<TResult> completeAction, Action<HttpError> failAction)
    {
      IssueRequest(
          c => c.PostAsync(address, getPayload()),
          completeAction,
          failAction
      );
    }

    /// <summary>
    /// Issues a request (DELETE) for deleting an object
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="address">The address.</param>
    /// <param name="completeAction">An action to invoke when the request completes.</param>
    /// <param name="failAction">An action to invoke when the request fails.</param>
    public void Delete<TResult>(string address, Action<TResult> completeAction, Action<HttpError> failAction)
    {
      IssueRequest(
          c => c.DeleteAsync(address),
          completeAction,
          failAction
      );
    }
  }

  public class HttpHelperAsync: HttpHelper
  {
    public static void IssueRequest<TResult>(Func<HttpClient, Task<HttpResponseMessage>> request, Action<TResult> completeAction,
                                            Action<HttpError> failAction, CancellationToken cancellationToken)
    {
        var client =  HttpHelper.CreateClient(TimeSpan.FromSeconds(15));

        request(client).ContinueWith(httpResponseMessage =>
        {
          try
          {
            var result = httpResponseMessage.GetAwaiter().GetResult();

            if (result.IsSuccessStatusCode)
            {
              var resultContent = result.Content.ReadAsAsync<TResult>(CancellationToken.None);

              completeAction?.Invoke(resultContent.Result);
              return;
            }

            failAction?.Invoke(result.Content.ReadAsAsync<HttpError>().Result);
          }
          catch (AggregateException ex)
          {
            if (failAction != null) failAction(new HttpError(ex.InnerExceptions[0], true));
          }
              // ReSharper disable once CatchAllClause
          catch (Exception ex)
          {
            failAction?.Invoke(new HttpError(ex, true));
          }

        },

        cancellationToken,
        TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.LazyCancellation | TaskContinuationOptions.HideScheduler,
        TaskScheduler.Default

        );

    }
  }
}
