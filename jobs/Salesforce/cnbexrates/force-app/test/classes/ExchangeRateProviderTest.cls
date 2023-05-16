@isTest
public class ExchangeRateProviderTest {

  // checks a new instance is created
  @isTest
  public static void getInstance() {
    Test.startTest();
    ExchangeRateProvider checkClass = ExchangeRateProvider.getInstance();
    Test.stopTest();

    System.assertEquals(
      null,
      ExchangeRateProvider.mockProvider,
      'The class is set as mock'
    );
  }

  // checks the mock instance is set
  @isTest
  public static void getInstance_Mock() {
    ExchangeRateProvider mockClass = new ExchangeRateProvider();

    Test.startTest();
    ExchangeRateProvider.setMock(mockClass);
    ExchangeRateProvider checkClass = ExchangeRateProvider.getInstance();
    Test.stopTest();

    System.assertNotEquals(
      null,
      ExchangeRateProvider.mockProvider,
      'The class is not set as mock'
    );
  }

  // checks the rates are parsed correctly from the API
  @isTest
  public static void getExchangeRates() {
    Test.setMock(HttpCalloutMock.class, new TestMocks.MockCallout(200));

    Test.startTest();
    List<ExchangeRate> rates = ExchangeRateProvider.getInstance()
      .getExchangeRates();
    Test.stopTest();

    System.assertEquals(3, rates.size(), 'There are not 3 returned rates');
    System.assertEquals(
      'AUD',
      rates[0].currencyCode,
      'The first rate is not for AUD'
    );
    System.assertEquals(
      14.489,
      rates[0].rate,
      'The AUD/CZK rate is not 14.489'
    );
  }

  // checks a 400 error response is throwing an exception
  @isTest
  public static void getExchangeRates_ErrorCode() {
    Exception error;
    Test.setMock(HttpCalloutMock.class, new TestMocks.MockCallout(400));

    Test.startTest();
    try {
      List<ExchangeRate> rates = ExchangeRateProvider.getInstance()
        .getExchangeRates();
    } catch (Exception ex) {
      error = ex;
    }
    Test.stopTest();

    System.assertEquals(
      'ExchangeRateProvider.ExchangeRatesException',
      error.getTypeName(),
      'The exception is not ExchangeRatesException'
    );
    System.assertEquals(
      'VALIDATION_ERROR: ValidationErrorCode: typeMismatch;  Field: lang; Value: i_am_not_valid_lang',
      error.getMessage(),
      'The exception message is not Field: lang; Value: i_am_not_valid_lang'
    );
  }

  // checks an empty response is throwing a generic exception
  @isTest
  public static void getExchangeRates_EmptyBody() {
    Exception error;
    Test.setMock(HttpCalloutMock.class, new TestMocks.MockCallout(418));

    Test.startTest();
    try {
      List<ExchangeRate> rates = ExchangeRateProvider.getInstance()
        .getExchangeRates();
    } catch (Exception ex) {
      error = ex;
    }
    Test.stopTest();

    System.assertEquals(
      'ExchangeRateProvider.ExchangeRatesException',
      error.getTypeName(),
      'The exception is not ExchangeRatesException'
    );
    System.assertEquals(
      'An unknown API error has occurred.',
      error.getMessage(),
      'The exception message is not An unknown API error has occurred.'
    );
  }
}
