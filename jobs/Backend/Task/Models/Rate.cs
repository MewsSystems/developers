using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models
{
    internal class Rate
    {
        string Country { get; init; }
        string Currency { get; init; }
        int Amount { get; init; }
        string Code { get; init; }
        double rate { get; init; }

        public Rate(string country, string currency, int amount, string code, double rate)
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
