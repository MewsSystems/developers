using CnbServiceClient.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CnbServiceClientTests.Extensions
{
	public class ServiceCollectionExtensionTests
    {
        private string serviceClientUrl = "ServiceClient:Url";
        private string serviceClientUrlValue = "http://localhost";

		[Test]
		public void AddCnbServiceClient_Runs_Successfully()
        {
            // Arrange
            var appSettings = new Dictionary<string, string>
            {
                { serviceClientUrl, serviceClientUrlValue },
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();

            var serviceCollection = new ServiceCollection();

            // Act
            Action act = () => serviceCollection.AddCnbServiceClient(configuration);

            // Assert
            act.Should().NotThrow();
        }

        [Test]
        public void AddCnbServiceClient_ThrowsException_WhenThereIsNoSettings()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .Build();

            var serviceCollection = new ServiceCollection();

            // Act
            Action act = () => serviceCollection.AddCnbServiceClient(configuration);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}

