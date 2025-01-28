using System;
namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public class CzechNationalBankSourceOptions
{
    public required Uri MainDataSourceUrl { get; set; } 
    public required Uri SecondaryDataSourceUrl { get; set; } 
}