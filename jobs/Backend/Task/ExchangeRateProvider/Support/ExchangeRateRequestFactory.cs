using System;
using System.Net.Http;

namespace ExchangeRateUpdater.Support;

public class ExchangeRateRequestFactory : IRequestFactory
{
  public HttpRequestMessage BuildRequest()
  {
    return new HttpRequestMessage() {
      RequestUri = new Uri("cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml", UriKind.Relative)
    };
  }
}