using System.Runtime.InteropServices;
using ZiggyCreatures.Caching.Fusion;

namespace ExchangeRateProviders.Czk.Config
{
	/// <summary>
	/// Provides intelligent caching strategies for Czech National Bank (CNB) exchange rate data
	/// based on their publication schedule and Prague timezone.
	/// </summary>
	public static class CnbCacheStrategy
	{
		/// <summary>
		/// Determines cache duration based on Prague time and CNB publication schedule.
		/// CNB publishes rates after 2:30 PM Prague time on working days only.
		/// Strategy:
		/// - Between 2:31 PM - 3:31 PM (weekdays): 5 minutes (fresh data likely available)
		/// - Other weekday times: 1 hour (data is stable)
		/// - Weekends: 12 hours (no new data ever released)
		/// </summary>
		public static FusionCacheEntryOptions GetCacheOptionsBasedOnPragueTime()
		{
			var pragueTime = GetPragueTime();
			var isWeekend = pragueTime.DayOfWeek == DayOfWeek.Saturday || pragueTime.DayOfWeek == DayOfWeek.Sunday;

			TimeSpan duration;

			if (isWeekend)
			{
				// Weekends: CNB never releases new data, cache for 12 hours to avoid unnecessary API calls
				duration = TimeSpan.FromHours(12);
			}
			else if (IsWithinPublishWindow(pragueTime))
			{
				// Publication window (2:31 PM - 3:31 PM): refresh frequently to catch new data
				duration = TimeSpan.FromMinutes(5);
			}
			else
			{
				// Outside publication window on weekdays: data is stable, cache for 1 hour
				duration = TimeSpan.FromHours(1);
			}

			return new FusionCacheEntryOptions
			{
				Duration = duration,
				// Allow serving stale data if fetch fails (fallback for up to 2x the normal duration)
				FailSafeMaxDuration = TimeSpan.FromTicks(duration.Ticks * 2)
			};
		}

		/// <summary>
		/// Gets current time in Prague timezone (Central European Time).
		/// </summary>
		private static DateTime GetPragueTime()
		{
			var pragueTimeZone = GetPragueTimeZone();
			return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pragueTimeZone);
		}

		/// <summary>
		/// Checks if current Prague time is within the CNB publication window (2:31 PM - 3:31 PM).
		/// </summary>
		private static bool IsWithinPublishWindow(DateTime pragueTime)
		{
			var time = pragueTime.TimeOfDay;
			var publishStart = new TimeSpan(14, 31, 0); // 2:31 PM
			var publishEnd = new TimeSpan(15, 31, 0);   // 3:31 PM

			return time >= publishStart && time <= publishEnd;
		}

		/// <summary>
		/// Gets Prague timezone using OS-specific timezone resolution.
		/// Supports Windows and Unix-like systems (Linux/macOS).
		/// </summary>
		private static TimeZoneInfo GetPragueTimeZone()
		{
			string timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
				? "Central Europe Standard Time"  // Windows format
				: "Europe/Prague";                // IANA format (Linux/macOS)

			return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
		}
	}
}