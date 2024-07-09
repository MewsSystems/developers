using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Lib.Shared
{
    public class FixedWindowRateLimiter : IFixedWindowRateLimiter
    {
        private readonly int _permitLimit;
        private readonly TimeSpan _window;
        private readonly bool _autoReplenishment;
        private readonly Timer _timer;
        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentQueue<DateTime> _requestTimestamps;

        public FixedWindowRateLimiter(FixedWindowRateLimiterOptions options)
        {
            _permitLimit = options.PermitLimit;
            _window = options.Window;
            _autoReplenishment = options.AutoReplenishment;
            _requestTimestamps = new ConcurrentQueue<DateTime>();
            _semaphore = new SemaphoreSlim(_permitLimit, _permitLimit);

            if (_autoReplenishment)
            {
                _timer = new Timer(ResetLimits, null, _window, _window);
            }
        }

        public async Task<bool> WaitAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var now = DateTime.UtcNow;

                // Remove expired requests
                while (_requestTimestamps.TryPeek(out var timestampExpired) && now - timestampExpired > _window)
                {
                    _requestTimestamps.TryDequeue(out _);
                    _semaphore.Release();
                }

                // Wait for a permit to be available
                await _semaphore.WaitAsync(cancellationToken);

                if (_requestTimestamps.Count < _permitLimit)
                {
                    _requestTimestamps.Enqueue(now);
                    return true;
                }

                // If permits are exhausted, release and block until a permit becomes available
                _semaphore.Release();

                // Use TryPeek to get the first item safely
                if (_requestTimestamps.TryPeek(out var timestamp))
                {
                    await Task.Delay(_window - (now - timestamp), cancellationToken);
                }
                else
                {
                    // If the queue is empty, theoretically this could mean all permits are available, so no delay should be necessary.
                    await Task.Delay(_window, cancellationToken);
                }
            }
        }

        private void ResetLimits(object state)
        {
            while (_requestTimestamps.TryPeek(out var timestamp) && DateTime.UtcNow - timestamp > _window)
            {
                _requestTimestamps.TryDequeue(out _);
                _semaphore.Release();
            }
        }
    }

    public class FixedWindowRateLimiterOptions
    {
        public int PermitLimit { get; set; }
        public TimeSpan Window { get; set; }
        public bool AutoReplenishment { get; set; }
    }
}