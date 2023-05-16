public class ExchangeRate {
  public Integer amount;
  public Decimal rate;

  @AuraEnabled
  public Date validFor;
  @AuraEnabled
  public String currencyCode;
  @AuraEnabled
  public Decimal exactRate {
    get {
      return this.rate != null &&
        this.amount != null
        ? this.rate / this.amount
        : null;
    }
  }
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
