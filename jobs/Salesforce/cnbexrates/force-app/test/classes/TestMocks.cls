@isTest
public class TestMocks {

  public class MockProvider implements System.StubProvider {
        
    public Object handleMethodCall(Object stubbedObject, String stubbedMethodName, 
      Type returnType, List<Type> listOfParamTypes, List<String> listOfParamNames, 
      List<Object> listOfArgs) {

      List<ExchangeRate> rates = new List<ExchangeRate>();

      for(String code : new List<String>{ 'ABC', 'DEF', 'GHI' }) {
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

  public class MockProviderError implements System.StubProvider {
      
    public Object handleMethodCall(Object stubbedObject, String stubbedMethodName, 
      Type returnType, List<Type> listOfParamTypes, List<String> listOfParamNames, 
      List<Object> listOfArgs) {

      throw new ExchangeRateProvider.ExchangeRatesException('Test message');
    }
  }

  public class CnbExchangeRatesMockEmptyBody implements HttpCalloutMock {

    private Integer statusCode;

    public CnbExchangeRatesMockEmptyBody(Integer statusCode) {
      this.statusCode = statusCode;
    }

    public HttpResponse respond(HttpRequest request) {
      HttpResponse response = new HttpResponse();
      response.setHeader('Content-Type', 'application/json');
      response.setStatusCode(this.statusCode);
      return response;
    }
  }

  public static HttpCalloutMock getApiExchangeRatesMock(Integer statusCode) {
    if(statusCode == 200 || statusCode == 400) {
      StaticResourceCalloutMock mock = new StaticResourceCalloutMock();
      mock.setHeader('Content-Type','application/json');
      mock.setStatusCode(statusCode);
      mock.setStaticResource(statusCode == 200 ? 'CnbExchangeRatesResponseMock' : 'CnbExchangeRatesResponseMockError');
      return mock;
    }

    else {
      return new CnbExchangeRatesMockEmptyBody(statusCode);
    }
  }
  

}