using System.Reflection;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;

public class IsoDateOnlyUrlParameterFormatter : DefaultUrlParameterFormatter
{
    public override string? Format(object? parameterValue, ICustomAttributeProvider attributeProvider, Type type)
    {
        if (typeof(DateOnly).IsAssignableFrom(type) && (parameterValue is not null))
        {
            return ((DateOnly)parameterValue).ToString("yyyy-MM-dd");
        }

        return base.Format(parameterValue, attributeProvider, type);
    }
}