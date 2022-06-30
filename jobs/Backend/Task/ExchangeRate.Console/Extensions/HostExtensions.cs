using ExchangeRate.Provider.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRate.Console.Extensions;

public static class HostExtensions
{
    /// <summary>
    ///     Validates all configurations that are using <see cref="IValidatable" /> interface
    /// </summary>
    public static IHost ValidateConfigurations(this IHost host)
    {
        var validatableObjects = host.Services.GetServices<IValidatable>();

        foreach (var validatableObject in validatableObjects)
            validatableObject.Validate();

        return host;
    }
}