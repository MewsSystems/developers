﻿namespace Domain.Ports;

public interface IExchangeRateApiSettings
{
    public string ApiBaseAddress { get; }
    public string Delimiter { get; }
    public string DecimalSeparator { get; }
}