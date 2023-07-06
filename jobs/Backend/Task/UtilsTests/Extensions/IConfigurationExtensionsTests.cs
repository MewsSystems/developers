using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Utils.Extensions;

namespace UtilsTests.Extensions
{
	public class IConfigurationExtensionsTests
	{
        private Dictionary<string, string> appSettings;
        private IConfigurationRoot configuration;
        private string Setting = "Test";

        [SetUp]
        public void SetUp()
        {
            appSettings = new Dictionary<string, string>
            {
                { Setting, Setting }
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();
        }

        [Test]
		public void GetRequiredValue_Successfully()
		{
            // Arrange

            // Act
            var result = configuration.GetRequiredValue<string>(Setting);

            // Assert
            result.Should().Be(Setting);
        }

        [Test]
        public void GetRequiredValue_Thorws_Exception()
        {
            // Arrange
            string key = "test1";

            // Act
            Action act = () => configuration.GetRequiredValue<string>(key);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}

