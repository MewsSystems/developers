using ExchangeRateProvider.Infrastructure.Policies;
using Microsoft.Extensions.Logging.Testing;
using Polly;
using Polly.Registry;
using Shouldly;
using System.Net;

namespace ExchangeRateProvider.Infrastructure.Tests.Unit.Policies;

public class PollyRegistryExtensionsTests
{
    [Fact]
    public void AddBasicRetryPolicy_ShouldRegisterPolicyWithCorrectName()
    {
        // Arrange
        var policyRegistry = new PolicyRegistry();

        // Act
        policyRegistry.AddBasicRetryPolicy();

        // Assert
        policyRegistry.ContainsKey(PolicyNames.WaitAndRetry).ShouldBeTrue();
        var policy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.WaitAndRetry);
        policy.ShouldNotBeNull();
    }

    [Fact]
    public async Task AddBasicRetryPolicy_ShouldNotRetryOnSuccessStatusCode()
    {
        // Arrange
        var policyRegistry = new PolicyRegistry();
        policyRegistry.AddBasicRetryPolicy();
        var policy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.WaitAndRetry);

        var attemptCount = 0;
        var fakeLogger = new FakeLogger();
        var context = new Context(PolicyNames.WaitAndRetry, new Dictionary<string, object>
        {
            { PolicyContextItems.Logger, fakeLogger }
        });

        // Act
        var result = await policy.ExecuteAsync(ctx =>
        {
            attemptCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }, context);

        // Assert
        attemptCount.ShouldBe(1);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        fakeLogger.Collector.Count.ShouldBe(0);
    }

    [Fact]
    public async Task AddBasicRetryPolicy_ShouldLogRetryAttemptNumber()
    {
        // Arrange
        var policyRegistry = new PolicyRegistry();
        policyRegistry.AddBasicRetryPolicy();
        var policy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.WaitAndRetry);

        var fakeLogger = new FakeLogger();
        var context = new Context(PolicyNames.WaitAndRetry, new Dictionary<string, object>
        {
            { PolicyContextItems.Logger, fakeLogger }
        });

        // Act
        await policy.ExecuteAsync(ctx =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)),
            context);

        // Assert
        var logs = fakeLogger.Collector.GetSnapshot().ToList();
        logs.Count.ShouldBe(3); // 3 retry attempts logged

        // Verify retry attempt numbers
        logs[0].Message.ShouldContain("retry 1");
        logs[1].Message.ShouldContain("retry 2");
        logs[2].Message.ShouldContain("retry 3");
    }
}