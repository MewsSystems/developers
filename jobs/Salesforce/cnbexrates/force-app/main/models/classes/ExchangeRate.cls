/**
 * Object to store and calculate rates to and from CZK.
 */
public class ExchangeRate {
  public Integer amount;
  public Decimal rate;

  @AuraEnabled
  public Date validFor;

  @AuraEnabled
  public String currencyCode;

  // 1 unit of foreign currency to CZK
  @AuraEnabled
  public Decimal exactRate {
    get {
      return this.rate != null &&
        this.amount != null
        ? this.rate / this.amount
        : null;
    }
  }

  // 1 CZK to foreign currency
  @AuraEnabled
  public Decimal invertedRate {
    get {
      return this.rate != null &&
        this.amount != null
        ? this.amount / this.rate
        : null;
    }
  }
}
