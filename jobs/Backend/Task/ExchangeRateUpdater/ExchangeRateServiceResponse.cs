namespace ExchangeRateUpdater
{
   public class ExchangeRateServiceResponse
   {
      public ExchangeRateResponse[] Rates { get; set; }
   }

   public class ExchangeRateResponse
   {
      public string CurrencyCode { get; set; }
      public decimal Rate { get; set; }
   }
}