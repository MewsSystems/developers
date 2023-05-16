/**
 * Server-side controller for the cnbExchangeRates LWC.
 */
public class ExchangeRateController {

  /**
   * Fetches CNB exchange rates.
   *
   * @return List of calculated exchange rates
   * @throw User-friendly AuraHandledException when an exception is caught
   * @since ver 0.1
   * @version ver 0.1
   */
  @AuraEnabled(cacheable=true)
  public static List<ExchangeRate> getExchangeRates() {
    try {
      return ExchangeRateProvider.getInstance().getExchangeRates();
    } catch (Exception e) {
      throw new AuraHandledException(e.getMessage());
    }
  }
}
