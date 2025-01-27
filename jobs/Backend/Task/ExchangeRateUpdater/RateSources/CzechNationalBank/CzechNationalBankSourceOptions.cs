using System;
namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public class CzechNationalBankSourceOptions
{
    public required Uri MainDataSourceUrl { get; init; } 
    public required Uri SecondaryDataSourceUrl { get; init; } 
}