require_relative 'date_time/compatibility'

# This file is maintained for backward compatibility
# The actual implementation is in the date_time directory
# See README.md for details on the new structure

module Utils
  # Module to handle time object creation and manipulation
  module TimeConstructionHelper
    # Helper method to create a Time object with the given components
    # @param year [Integer] Year
    # @param month [Integer] Month
    # @param day [Integer] Day
    # @param hour [Integer] Hour
    # @param min [Integer] Minute
    # @param sec [Integer] Second
    # @param timezone [String] Timezone offset string
    # @return [Time] Time object with the given components
    def self.create_time_with_components(year, month, day, hour, min, sec, timezone)
      Time.new(year, month, day, hour, min, sec, timezone)
    end
  end

  # Module to handle business day calculations
  module BusinessDayHelper
    # Get the next business day, skipping weekends if necessary
    # @param time [Time, Date] The base time
    # @param working_days_only [Boolean] Whether to adjust for weekends
    # @return [Time, Date] The adjusted time
    def self.next_business_day(time, working_days_only = true)
      return time unless working_days_only

      # Handle Date objects specifically
      if time.is_a?(Date)
        date = time
        # Skip weekends
        date = date + 2 if date.saturday? # Saturday to Monday
        date = date + 1 if date.sunday?   # Sunday to Monday
        return date
      end

      # Get day of week (0 = Sunday, 6 = Saturday)
      wday = time.wday

      case wday
      when 0 # Sunday - move to Monday (+1 day)
        TimeConstructionHelper.create_time_with_components(
          time.year, time.month, time.day + 1,
          time.hour, time.min, time.sec, time.strftime('%:z')
        )
      when 6 # Saturday - move to Monday (+2 days)
        # Ensure we're moving to Monday of next week, not skipping to next week
        next_day = time + (2 * 86400) # add 2 days

        # If this puts us in a different month or causes other date issues,
        # manually construct the date to ensure we get the right Monday
        if next_day.day < time.day # We crossed a month boundary
          # For Saturday, we want the Monday that is 2 days ahead (not the next week)
          TimeConstructionHelper.create_time_with_components(
            time.year, time.month, time.day + 2,
            time.hour, time.min, time.sec, time.strftime('%:z')
          )
        else
          next_day
        end
      else
        time # Weekday, no adjustment needed
      end
    end

    # Get the previous business day, skipping weekends if necessary
    # @param date [Date] The base date
    # @param working_days_only [Boolean] Whether to adjust for weekends
    # @return [Date] The previous business day
    def self.previous_business_day(date, working_days_only = true)
      return date - 1 unless working_days_only

      prev_date = date - 1

      # Skip weekends
      prev_date -= 2 if prev_date.sunday?
      prev_date -= 1 if prev_date.saturday?

      prev_date
    end

    # Adjust time for working days if needed
    # @param time [Time] The time to adjust
    # @param working_days_only [Boolean] Whether to adjust for weekends
    # @return [Time] Adjusted time
    def self.adjust_for_working_days(time, working_days_only = true)
      working_days_only ? next_business_day(time, working_days_only) : time
    end
  end

  # Module to handle publication time scheduling
  module PublicationTimeHelper
    # Calculate next publication time based on update frequency
    # @param update_frequency [Symbol] Update frequency (:daily, :hourly, etc.)
    # @param publication_time [Time] Base publication time
    # @param current_time [Time] Current time
    # @return [Time] Next publication time
    def self.calculate_next_publication(update_frequency, publication_time, current_time = Time.now)
      return nil unless publication_time

      case update_frequency
      when :daily
        calculate_next_daily_publication(publication_time, current_time)
      when :hourly
        calculate_next_hourly_publication(publication_time, current_time)
      when :minute
        current_time + 60 # Simply add one minute
      else
        current_time + 3600 # Default to adding one hour
      end
    end

    # Calculate next daily publication time
    # @param publication_time [Time] Base publication time
    # @param current_time [Time] Current time
    # @return [Time] Next publication time
    def self.calculate_next_daily_publication(publication_time, current_time)
      publ_hour = publication_time.hour
      publ_min = publication_time.min
      publ_tz = publication_time.strftime('%:z')

      # Create publication time for today
      today_pub = TimeConstructionHelper.create_time_with_components(
        current_time.year, current_time.month, current_time.day,
        publ_hour, publ_min, 0, publ_tz
      )

      if current_time < today_pub
        # Today's publication hasn't happened yet
        today_pub
      else
        # Today's publication has passed, get next day's
        next_day = current_time + 86400 # add 1 day in seconds
        next_day_pub = TimeConstructionHelper.create_time_with_components(
          next_day.year, next_day.month, next_day.day,
          publ_hour, publ_min, 0, publ_tz
        )

        next_day_pub
      end
    end

    # Calculate next hourly publication time
    # @param publication_time [Time] Base publication time with minutes
    # @param current_time [Time] Current time
    # @return [Time] Next hourly publication time
    def self.calculate_next_hourly_publication(publication_time, current_time)
      hour = current_time.hour
      next_hour = (hour + 1) % 24

      # Handle day boundary
      day_offset = next_hour < hour ? 1 : 0
      next_day = current_time + (day_offset * 86400)

      # Create publication time for next hour
      TimeConstructionHelper.create_time_with_components(
        next_day.year, next_day.month, next_day.day,
        next_hour, publication_time.min, 0, publication_time.strftime('%:z')
      )
    end

    # Create a Time object for publication on a given date
    # @param date [Date] The date
    # @param hour [Integer] Hour
    # @param minute [Integer] Minute
    # @param timezone [String] Timezone
    # @return [Time] Time object for the publication
    def self.publication_time_for_date(date, hour, minute, timezone)
      Time.new(date.year, date.month, date.day, hour, minute, 0, timezone)
    end
  end

  # Module to handle cache TTL calculations
  module CacheTTLHelper
    # Calculate cache TTL until next publication
    # @param update_frequency [Symbol] Update frequency
    # @param publication_time [Time] Publication time
    # @param current_time [Time] Current time
    # @param default_ttl [Integer] Default TTL if calculation fails
    # @return [Integer] TTL in seconds
    def self.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time = Time.now,
                                                default_ttl = 3600)
      next_pub = PublicationTimeHelper.calculate_next_publication(update_frequency, publication_time, current_time)
      # If next publication time exists, use time until then (min 60 seconds)
      # Otherwise fall back to default
      next_pub ? [(next_pub - current_time).to_i, 60].max : default_ttl
    end

    # Get default TTL values by update frequency
    # @return [Hash] Default TTL values in seconds
    def self.get_default_ttls
      {
        realtime: 30,     # 30 seconds
        minute: 30,       # 30 seconds
        hourly: 15 * 60,  # 15 minutes
        daily: 3600       # 1 hour (default if publication time not available)
      }
    end
  end
  
  # Module to handle fetching date determination based on metadata
  module FetchDateHelper
    # Determine the appropriate date to fetch rates for based on metadata
    # @param metadata [Hash] Provider metadata
    # @return [Date] The date to fetch rates for
    def self.determine_fetch_date(metadata)
      today = Date.today
      now = Time.now

      # Use default logic if metadata is missing
      return today unless metadata

      update_frequency = metadata[:update_frequency]
      publication_time = metadata[:publication_time]
      working_days_only = metadata[:working_days_only]

      # For update frequencies more frequent than daily, we always use today's date
      # as the repository will handle data versioning based on date+time
      unless update_frequency == :daily
        return today
      end

      # For daily updates, check if today's data should be available yet
      if publication_time
        # If current time is before today's publication time, use previous business day
        if now < publication_time
          # Go to previous day (and if working days only, ensure it's a working day)
          previous_date = BusinessDayHelper.previous_business_day(today, working_days_only)

          return previous_date
        end
      end

      # Default: use today's date
      today
    end
  end

  # Compatibility module that provides backward compatibility with the original DateTimeHelper
  module DateTimeHelper
    def self.create_time_with_components(year, month, day, hour, min, sec, timezone)
      TimeConstructionHelper.create_time_with_components(year, month, day, hour, min, sec, timezone)
    end

    def self.next_business_day(time, working_days_only = true)
      BusinessDayHelper.next_business_day(time, working_days_only)
    end

    def self.previous_business_day(date, working_days_only = true)
      BusinessDayHelper.previous_business_day(date, working_days_only)
    end

    def self.adjust_for_working_days(time, working_days_only = true)
      BusinessDayHelper.adjust_for_working_days(time, working_days_only)
    end

    def self.calculate_next_publication(update_frequency, publication_time, current_time = Time.now)
      PublicationTimeHelper.calculate_next_publication(update_frequency, publication_time, current_time)
    end

    def self.calculate_next_daily_publication(publication_time, current_time)
      PublicationTimeHelper.calculate_next_daily_publication(publication_time, current_time)
    end

    def self.calculate_next_hourly_publication(publication_time, current_time)
      PublicationTimeHelper.calculate_next_hourly_publication(publication_time, current_time)
    end

    def self.publication_time_for_date(date, hour, minute, timezone)
      PublicationTimeHelper.publication_time_for_date(date, hour, minute, timezone)
    end

    def self.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time = Time.now, default_ttl = 3600)
      CacheTTLHelper.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time, default_ttl)
    end

    def self.get_default_ttls
      CacheTTLHelper.get_default_ttls
    end

    def self.determine_fetch_date(metadata)
      FetchDateHelper.determine_fetch_date(metadata)
    end
  end
end