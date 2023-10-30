namespace Data.Models;

public abstract class BaseEntity
{
    public DateTime CreatedDate { get; set; }
}

public class CurrencyCzechRate : BaseEntity
{
    public CurrencyCzechRate(string country, string currency, decimal amount, string code, decimal rate, DateTime createdDate)
    {
        CreatedDate = createdDate;
        Country = country;
        Currency = currency;
        Amount = amount;
        Code = code;
        Rate = rate;
    }   
    public string Country { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public string Code { get; set; }
    public decimal Rate { get; set; }
}
