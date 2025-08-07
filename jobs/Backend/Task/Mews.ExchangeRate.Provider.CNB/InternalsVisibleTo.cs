
using System.Runtime.CompilerServices;

// Internal types must be visible for Autofixture - nsubstitute so it can properly mock and create services for tests
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
// Internal types must be visible for test project
[assembly: InternalsVisibleTo("Mews.ExchangeRate.Provider.CNB.UnitTests")]
[assembly: InternalsVisibleTo("Mews.ExchangeRate.Provider.CNB.IntegrationTests")]
[assembly: InternalsVisibleTo("Mews.ExchangeRate.API.IntegrationTests")]
