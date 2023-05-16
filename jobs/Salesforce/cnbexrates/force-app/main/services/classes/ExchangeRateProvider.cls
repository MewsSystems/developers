public class ExchangeRateProvider {

  public static ExchangeRateProvider getInstance() {
    return (ExchangeRateProvider)ServiceLocator.getInstance(ExchangeRateProvider.class);
  }

  public List<ExchangeRate> getExchangeRates() {
    List<ExchangeRate> allExchangeRates = new List<ExchangeRate>();

    HttpRequest request = new HttpRequest();
    request.setEndpoint('https://api.cnb.cz/cnbapi/exrates/daily?lang=EN');
    request.setMethod('GET');
       
    Http http = new Http();
    HttpResponse response = http.send(request);

    if(response.getStatusCode() == 200 && !String.isBlank(response.getBody())) {
      ExchangeRatesPayload allRatesPayload = (ExchangeRatesPayload)JSON.deserialize(
        response.getBody(),
        ExchangeRatesPayload.class
      );

      return allRatesPayload.rates;
    }

    else if(String.isBlank(response.getBody())) {
      throw new ExchangeRatesException('An unknown API error has occurred.');
    }

    else {
      ExchangeRatesError ratesError = (ExchangeRatesError)JSON.deserialize(
        response.getBody(),
        ExchangeRatesError.class
      );

      throw new ExchangeRatesException(ratesError.errorCode + ': ' + ratesError.description);
    }
  }

  public class ExchangeRatesException extends Exception {}

}