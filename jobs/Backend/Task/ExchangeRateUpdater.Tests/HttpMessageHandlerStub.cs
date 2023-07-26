using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests;

public class HttpMessageHandlerStub : HttpMessageHandler
{
   private string _response = "";

   public void AddResponse(string response)
   {
      _response = response;
   }

   protected override Task<HttpResponseMessage> SendAsync(
       HttpRequestMessage request,
       CancellationToken cancellationToken)
   {
      return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
      {
         Content = new StringContent(_response)
      });
   }
}