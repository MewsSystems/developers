using Dapper;
using System.Data;

namespace DataLayer.Dapper;

/// <summary>
/// Dapper TypeHandler for converting between SQL Server DATE columns and .NET DateOnly.
/// Required because Dapper doesn't have built-in support for DateOnly types.
/// </summary>
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        if (value is DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        if (value is DateOnly dateOnly)
        {
            return dateOnly;
        }

        throw new InvalidCastException($"Cannot convert {value?.GetType()?.Name ?? "null"} to DateOnly");
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }
}
