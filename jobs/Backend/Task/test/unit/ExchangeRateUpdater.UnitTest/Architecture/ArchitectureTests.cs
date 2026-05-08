using FluentAssertions;
using NetArchTest.Rules;

namespace ExchangeRateUpdater.UnitTest.Architecture
{
    /// <summary>
    /// Test class to ensure the Clean Architecture rules.
    /// </summary>
    public class ArchitectureTests
    {
        // Root namespaces of projects inside the solution
        private const string ApplicationNamespace = "ExchangeRateUpdater.Application";
        private const string InfrastructureNamespace = "ExchangeRateUpdater.Infrastructure";
        private const string ApiNamespace = "ExchangeRateUpdater.Api";

        [Fact]
        public void Domain_Should_Not_HaveDependenciesOnAnyProject()
        {
            // Arrange
            var domainAssembly = typeof(ExchangeRateUpdater.Domain.Const.ProviderConstants).Assembly;

            var otherProjects = new[]
            {
                ApplicationNamespace,
                InfrastructureNamespace,
                ApiNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_HaveDependenciesOnOtherProject()
        {
            // Arrange
            var applicationAssembly = typeof(ExchangeRateUpdater.Application.Const.CacheConstants).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNamespace,
                ApiNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(applicationAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Controllers_Should_HaveDependenciesOnMediatR()
        {
            // Arrange
            var apiAssembly = typeof(Api.Controllers.ExchangeRateController).Assembly;

            // Act
            var testResult = Types
                .InAssembly(apiAssembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
    }
}
