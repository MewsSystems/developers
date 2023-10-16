namespace Mews.ExchangeRateUpdater.Services.Models
{
    /// <summary>
    /// This is a model class in place, anticipating the future use of some sort of repository to store the exchange rates
    /// if needed, and this class can be considered as the template to store the entity or the domain object in the database
    /// </summary>
    public class ExchangeRateModel
    {
        public string ValidFor { get; set; }

        public string Country { get; set; }

        public CurrencyModel SourceCurrency { get; set; }

        public CurrencyModel TargetCurrency { get; set; } = new CurrencyModel { Currency = "koruna", Code = "CZK" };

        public int Amount { get; set; }

        public decimal Rate { get; set; }
    }
}
