using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Lib.Shared;
using NUnit.Framework;
using V1CzechNationBankExchangeRateProvider;

namespace ExchangeRateUpdate.CzechNationalBank.Test
{

    [TestFixture]
    public class FixedWindowRateLimiterTests
    {
        [Test]
        public async Task WaitAsync_AllowsRequests_UnderPermitLimit()
        {
            var options = new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(10),
                AutoReplenishment = false
            };

            var rateLimiter = new FixedWindowRateLimiter(options);

            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(await rateLimiter.WaitAsync());
            }
        }

        [Test]
        public async Task WaitAsync_BlocksRequests_OverPermitLimit()
        {
            var options = new FixedWindowRateLimiterOptions
            {
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(10),
                AutoReplenishment = false
            };

            var rateLimiter = new FixedWindowRateLimiter(options);

            Assert.IsTrue(await rateLimiter.WaitAsync());
            Assert.IsTrue(await rateLimiter.WaitAsync());

            var task = rateLimiter.WaitAsync();
            var completedTask = await Task.WhenAny(task, Task.Delay(1000));
            Assert.AreEqual(TaskStatus.WaitingForActivation, task.Status);
            Assert.AreNotEqual(task, completedTask);
        }

        [Test]
        public async Task WaitAsync_AllowsRequests_AfterWindowResets()
        {
            var options = new FixedWindowRateLimiterOptions
            {
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(1),
                AutoReplenishment = true
            };

            var rateLimiter = new FixedWindowRateLimiter(options);

            Assert.IsTrue(await rateLimiter.WaitAsync());
            Assert.IsTrue(await rateLimiter.WaitAsync());

            // Wait for window to reset
            await Task.Delay(1100);

            Assert.IsTrue(await rateLimiter.WaitAsync());
            Assert.IsTrue(await rateLimiter.WaitAsync());
        }

        [Test]
        public async Task WaitAsync_Cancellation_ThrowsOperationCanceledException()
        {
            var options = new FixedWindowRateLimiterOptions
            {
                PermitLimit = 1,
                Window = TimeSpan.FromSeconds(10),
                AutoReplenishment = false
            };

            var rateLimiter = new FixedWindowRateLimiter(options);

            Assert.IsTrue(await rateLimiter.WaitAsync());

            var cts = new CancellationTokenSource();
            var task = rateLimiter.WaitAsync(cts.Token);

            cts.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        }
    }

}