using System.Reflection;

namespace ExchangeRateService.Core;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetClassesAssignableTo<T>(this Assembly assembly) =>
        assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(T)));
}