using Entities.Concrete;

namespace Entities.Dtos
{
    public record CurrencyListRecord
    {
        public IEnumerable<Currency> Currencies { get; set; }
        public CurrencyListRecord(IEnumerable<Currency> currencies)
        {
            Currencies=currencies;
        }
    }
}
