﻿namespace ExchangeRateUpdater.Acceptance.Tests.Dtos;

internal class ExchangeRateDto
{
    public string? From { get; set; }
    public string? To { get; set; }
    public decimal ExchangeRate { get; set; }
}

