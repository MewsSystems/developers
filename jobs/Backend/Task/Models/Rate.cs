using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Models
{
    public class Rate
    {
        public string Country { get; init; }
        public string Currency { get; init; }
        public int Amount { get; init; }

        [StringLength(3, MinimumLength = 3)]
        public string Code { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "Rate must be a positive number")]
        public decimal rate { get; init; }

        public Rate(string country, string currency, int amount, string code, decimal rate)
        {
            Country = country;
            Currency = currency;
            Amount = amount;
            Code = code;
            this.rate = rate;
        }

        public override string ToString()
        {
            return $"Country = {Country}, Currency = {Currency}, Amount = {Amount}, Code = {Code}, rate = {rate}".ToString();
        }
    }
}
