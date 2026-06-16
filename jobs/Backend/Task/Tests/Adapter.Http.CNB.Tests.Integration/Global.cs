using Adapter.Http.CNB.Tests.Integration.Configuration;
using NUnit.Framework;

namespace Adapter.Http.CNB.Tests.Integration;

[SetUpFixture]
public class Global
{
    internal static Configuration.CNBSettings Settings { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Settings = CNBSettingsConfiguration.GetCNBSettings();
    }
}