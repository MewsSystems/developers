@isTest
public class TestMocks {

  // mock ExchangeRateProvider class
  public class MockProvider implements System.StubProvider {
    public Object handleMethodCall(
      Object stubbedObject,
      String stubbedMethodName,
      Type returnType,
      List<Type> listOfParamTypes,
      List<String> listOfParamNames,
      List<Object> listOfArgs
    ) {
      List<ExchangeRate> rates = new List<ExchangeRate>();

      for (String code : new List<String>{ 'ABC', 'DEF', 'GHI' }) {
        ExchangeRate rate = new ExchangeRate();
        rate.validFor = Date.today();
        rate.amount = 1;
        rate.rate = Decimal.valueOf(Math.random());
        rate.currencyCode = code;
        rates.add(rate);
      }

      return rates;
    }
  }

  // mock ExchangeRateProvider class throwing an exception
  public class MockProviderError implements System.StubProvider {
    public Object handleMethodCall(
      Object stubbedObject,
      String stubbedMethodName,
      Type returnType,
      List<Type> listOfParamTypes,
      List<String> listOfParamNames,
      List<Object> listOfArgs
    ) {
      throw new ExchangeRateProvider.ExchangeRatesException('Test message');
    }
  }

  // mock API responses
  public class MockCallout implements HttpCalloutMock {
    private Integer statusCode;

    public MockCallout(Integer statusCode) {
      this.statusCode = statusCode;
    }

    public HttpResponse respond(HttpRequest request) {
      HttpResponse response = new HttpResponse();
      response.setHeader('Content-Type', 'application/json');
      response.setStatusCode(this.statusCode);

      if (statusCode == 200) {
        response.setBody(
          '{"rates":[' +
            '{"validFor":"2023-05-15","order":92,"country":"Australia","currency":"dollar",' +
            '"amount":1,"currencyCode":"AUD","rate":14.489},' +
            '{"validFor":"2023-05-15","order":92,"country":"Hungary","currency":"forint",' +
            '"amount":100,"currencyCode":"HUF","rate":6.38},' +
            '{"validFor":"2023-05-15","order":92,"country":"Iceland","currency":"krona",' +
            '"amount":100,"currencyCode":"ISK","rate":15.616}' +
            ']}'
        );
      } else if (statusCode == 400) {
        response.setBody(
          '{"description": "ValidationErrorCode: typeMismatch;  Field: lang; Value: i_am_not_valid_lang",' +
            '"errorCode": "VALIDATION_ERROR",' +
            '"happenedAt": "2023-05-16 14:09:25",' +
            '"endPoint": "/cnbapi/exrates/daily?lang=i_am_not_valid_lang",' +
            '"messageId": "5WWfxPtOYHNA"}'
        );
      }

      return response;
    }
  }
}
