using System.Globalization;
using ExchangeRateUpdater.Models.Entities;

namespace ExchangeRateUpdater.Service.Tests.Unit;

public static class A
{
    public static readonly Currency TestSourceCurrency   = new("SRC");
    public static readonly Currency TestTargetCurrency   = new("TRG");
    public const decimal TestRateValue                   = 1.5m;

    public static string MappingDelimiter        = "|";
    public static string MappingDecimalSeparator = ".";
    public const string TimezoneId               = "Central Europe Standard Time";

    public static readonly DateTime TestDate = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(2022, 06, 01), TimeZoneInfo.FindSystemTimeZoneById(TimezoneId));
}