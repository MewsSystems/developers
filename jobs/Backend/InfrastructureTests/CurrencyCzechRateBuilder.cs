using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class CurrencyCzechRateBuilder
{
    protected readonly DbContext _dbContext;
    private decimal _amount;
    private decimal _rate;
    private string _country;
    private string _currency;
    private string _code;
    private DateTime _date;

    public CurrencyCzechRateBuilder(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public CurrencyCzechRate Create()
    {
        var altenarConfiguration = new CurrencyCzechRate(_country, _currency, _amount, _code, _rate, _date);
        _dbContext.Add(altenarConfiguration);
        _dbContext.SaveChanges();
        return altenarConfiguration;
    }

    public CurrencyCzechRateBuilder WithCreatedDate(DateTime date)
    {
        _date = date;
        return this;
    }
    public CurrencyCzechRateBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }
    public CurrencyCzechRateBuilder WithCurrency(string currency)
    {
        _currency = currency;
        return this;
    }
    public CurrencyCzechRateBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }
    public CurrencyCzechRateBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }
    public CurrencyCzechRateBuilder WithRate(decimal rate)
    {
        _rate = rate;
        return this;
    }
}
