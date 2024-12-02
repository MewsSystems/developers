using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Common;

public record Result<T>
{
    public bool IsSuccess { get; private init; }

    public T? Value { get; private init; }

    public string? Error { get; private init; }

    public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };

    public Result()
    {
    }
}