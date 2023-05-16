public class ExchangeRateProvider {

  // stores the mock provider class instance, if any
  @TestVisible
  private static Object mockProvider;

  /**
   * Sets a provider class instance as mock to be
   * used in unit tests.
   *
   * @param serviceImpl Class instance
   * @since ver 0.1
   * @version ver 0.1
   */
  @TestVisible
  private static void setMock(Object serviceImpl) {
    mockProvider = serviceImpl;
  }

  /**
   * Returns a mock instance if set, otherwise creates
   * a new instance of this class type.
   *
   * @return Class mock or new instance
   * @since ver 0.1
   * @version ver 0.1
   */
  public static ExchangeRateProvider getInstance() {
    return (ExchangeRateProvider) (mockProvider != null
      ? mockProvider
      : ExchangeRateProvider.class.newInstance());
  }

  /**
   * Gets the exchange rates from the CNB API and parses the
   * response into ExchangeRate objects with calculated rates
   * to and from CZK.
   *
   * @return List of calculated exchange rates
   * @throw ExchangeRatesException when the status code is not 200
   *        or the response body is empty
   * @since ver 0.1
   * @version ver 0.1
   */
  public List<ExchangeRate> getExchangeRates() {
    List<ExchangeRate> allExchangeRates = new List<ExchangeRate>();

    HttpRequest request = new HttpRequest();
    request.setEndpoint('https://api.cnb.cz/cnbapi/exrates/daily?lang=EN');
    request.setMethod('GET');

    Http http = new Http();
    HttpResponse response = http.send(request);

    if (response.getStatusCode() == 200 && !String.isBlank(response.getBody())) {
      ExchangeRatesPayload allRatesPayload = (ExchangeRatesPayload) JSON.deserialize(
        response.getBody(),
        ExchangeRatesPayload.class
      );

      return allRatesPayload.rates;
    } 
    
    else if (String.isBlank(response.getBody())) {
      throw new ExchangeRatesException('An unknown API error has occurred.');
    } 
    
    else {
      ExchangeRatesError ratesError = (ExchangeRatesError) JSON.deserialize(
        response.getBody(),
        ExchangeRatesError.class
      );

      throw new ExchangeRatesException(
        ratesError.errorCode + ': ' + ratesError.description
      );
    }
  }

  // custom Exception type
  public class ExchangeRatesException extends Exception {}
}
