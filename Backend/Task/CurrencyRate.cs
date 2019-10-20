namespace ExchangeRateUpdater
{
  class CurrencyRate
  {
    public string Code { get; set; }
    public decimal Rate { get; set; }
    public int Count { get; set; }
    public bool isValid => Code != null && Rate != -1 && Count != -1;
  }
}