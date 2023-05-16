@isTest
public class ExchangeRateTest {
  @isTest
  public static void checkCalculatedRates() {
    ExchangeRate rate = new ExchangeRate();
    rate.amount = 100;
    rate.rate = 5;

    Test.startTest();
    Decimal exactRate = rate.exactRate;
    Decimal invertedRate = rate.invertedRate;
    Test.stopTest();

    System.assertEquals(0.05, exactRate, 'The exact rate is not 0.05');
    System.assertEquals(20, invertedRate, 'The inverted rate is not 20');
  }

  @isTest
  public static void checkCalculatedRates_AmountNull() {
    ExchangeRate rate = new ExchangeRate();
    rate.amount = null;
    rate.rate = 5;

    Test.startTest();
    Decimal exactRate = rate.exactRate;
    Decimal invertedRate = rate.invertedRate;
    Test.stopTest();

    System.assertEquals(null, exactRate, 'The exact rate is not null');
    System.assertEquals(null, invertedRate, 'The inverted rate is not null');
  }

  @isTest
  public static void checkCalculatedRates_RateNull() {
    ExchangeRate rate = new ExchangeRate();
    rate.amount = 100;
    rate.rate = null;

    Test.startTest();
    Decimal exactRate = rate.exactRate;
    Decimal invertedRate = rate.invertedRate;
    Test.stopTest();

    System.assertEquals(null, exactRate, 'The exact rate is not null');
    System.assertEquals(null, invertedRate, 'The inverted rate is not null');
  }
}
