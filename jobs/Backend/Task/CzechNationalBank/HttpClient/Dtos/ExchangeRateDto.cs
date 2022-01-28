﻿#nullable enable
namespace ExchangeRateUpdater.CzechNationalBank.HttpClient.Dtos
{
    public class ExchangeRateDto
    {
        public string? Country { get; set; }
        public string? CurrencyName { get; set; }
        public int? Amount { get; set; }
        public Currency? Currency { get; set; }
        public decimal? Rate { get; set; }
    }
}