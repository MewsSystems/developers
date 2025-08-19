using System;

namespace ExchangeRateUpdater.Errors;

public class CnbException : Exception
{
    public CnbErrorCode ErrorCode { get; }

    public CnbException(CnbErrorCode errorCode, string message = null, Exception innerException = null)
        : base(message ?? errorCode.ToString(), innerException)
    {
        ErrorCode = errorCode;
    }
}

public enum CnbErrorCode
{
    EmptyResponse,
    InsufficientData,
    InvalidDateFormat,
    InvalidHeaderFormat,
    NoValidRates,
    NetworkError,
    TimeoutError,
    ParsingError,
    UnexpectedError
}
