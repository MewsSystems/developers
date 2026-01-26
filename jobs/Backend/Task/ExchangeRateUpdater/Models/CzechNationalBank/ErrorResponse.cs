using System;

namespace ExchangeRateUpdater.Models.CzechNationalBank;

public record ErrorResponse(string description, string endPoint, string errorCode, DateTime happenedAt, string messageId);