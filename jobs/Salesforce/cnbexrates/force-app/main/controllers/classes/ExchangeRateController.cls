public class ExchangeRateController {

  @AuraEnabled(cacheable=true)
  public static List<ExchangeRate> getExchangeRates() {
    try {
      return ExchangeRateProvider.getInstance().getExchangeRates();
    } catch (Exception e) {
      throw new AuraHandledException(e.getMessage());
    }
  }

}