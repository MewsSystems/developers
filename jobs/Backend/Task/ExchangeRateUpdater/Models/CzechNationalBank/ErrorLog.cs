using System.Net;

namespace ExchangeRateUpdater.Models.CzechNationalBank;

public record ErrorLog(string description, string errorCode, HttpStatusCode StatusCode);