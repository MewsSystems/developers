namespace Mews.ExchangeRateMonitor.Common.Domain.Results;

/// <summary>
/// Represents the outcome of an operation, encapsulating either a value of type <typeparamref name="TValue"/>
/// or one or more errors indicating failure.
/// </summary>
/// <typeparam name="TValue">The type of the value held by the result when the operation is successful.</typeparam>
/// <remarks>
/// The <see cref="Result{TValue}"/> type provides a mechanism to represent both successful outcomes,
/// which carry a value of type <typeparamref name="TValue"/>, and errors, which can be encapsulated in
/// <see cref="Error"/>, a collection of <see cref="Error"/>, or both. This abstraction aids in
/// avoiding exceptions for flow control and makes error handling explicit and structured.
/// </remarks>
public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Defines an implicit conversion operator that constructs a <see cref="Result{TValue}"/>
    /// from a value of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="value">The value of type <typeparamref name="TValue"/> used to create a successful result.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance representing a successful outcome with the provided value.</returns>
    public static implicit operator Result<TValue>(TValue value) => new(value);

    /// <summary>
    /// Defines an implicit conversion operator that constructs a <see cref="Result{TValue}"/>
    /// from an <see cref="Error"/> instance.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> instance encapsulating the error information
    /// used to create a failure result.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance representing a failure with the provided error.</returns>
    public static implicit operator Result<TValue>(Error error) => new(error);

    /// <summary>
    /// Defines implicit conversion operators for the <see cref="Result{TValue}"/> struct.
    /// </summary>
    /// <remarks>
    /// These operators allow for implicit conversion between <see cref="TValue"/>, <see cref="Error"/>,
    /// lists of <see cref="Error"/>, and arrays of <see cref="Error"/> into a <see cref="Result{TValue}"/> object.
    /// </remarks>
    /// <example>
    /// This conversion simplifies the process of creating instances of <see cref="Result{TValue}"/>
    /// by allowing direct assignment from supported types.
    /// </example>
    public static implicit operator Result<TValue>(List<Error> errors) => new(errors);

    /// <summary>
    /// Provides implicit conversion operators for the <see cref="Result{TValue}"/> type.
    /// </summary>
    public static implicit operator Result<TValue>(Error[] errors) => new([.. errors]);
}
