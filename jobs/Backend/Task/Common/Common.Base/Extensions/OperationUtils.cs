namespace Common.Base.Extensions;

public static class OperationUtils
{
    /// <summary>
    ///     Validates all values if any equals to provided value
    /// </summary>
    /// <param name="equalsTo">Provided value that any values must be equals to</param>
    /// <param name="values">Values to validate</param>
    /// <returns>Returns true if any value is equals to provided value</returns>
    public static bool EqualsAny<T>(T equalsTo, params T[] values)
    {
        return values.Any(value => value != null && value.Equals(equalsTo));
    }
}