namespace Mews.ExchangeRateMonitor.Common.Domain.Results;

public readonly record struct Error
{
    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    private Error(string code, string description, int numericType)
    {
        Code = code;
        Description = description;
        NumericType = numericType;

        Type = ErrorType.Custom;
    }

    /// <summary>
    /// Gets the unique error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Gets the numeric value of the type.
    /// </summary>
    public int NumericType { get; }

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error Unexpected(string code, string description) =>
        new(code, description, ErrorType.Unexpected);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);

    public static Error Custom(int type, string code, string description) =>
        new(code, description, type);

    public bool Equals(Error other)
    {
        return Type == other.Type &&
               NumericType == other.NumericType &&
               Code == other.Code &&
               Description == other.Description;
    }

    public override int GetHashCode() => HashCode.Combine(Code, Description, Type, NumericType);
}
