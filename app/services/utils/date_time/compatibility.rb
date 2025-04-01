require_relative 'time_constructor'
require_relative 'business_day_calculator'
require_relative 'publication_scheduler'
require_relative 'cache_ttl_calculator'
require_relative 'fetch_date_determinator'

module Utils
  # Compatibility module that provides backward compatibility with the original DateTimeHelper
  module DateTimeHelper
    def self.create_time_with_components(year, month, day, hour, min, sec, timezone)
      DateTime::TimeConstructor.create_with_components(year, month, day, hour, min, sec, timezone)
    end

    def self.next_business_day(time, working_days_only = true)
      DateTime::BusinessDayCalculator.next_business_day(time, working_days_only)
    end

    def self.previous_business_day(date, working_days_only = true)
      DateTime::BusinessDayCalculator.previous_business_day(date, working_days_only)
    end

    def self.adjust_for_working_days(time, working_days_only = true)
      DateTime::BusinessDayCalculator.adjust_for_working_days(time, working_days_only)
    end

    def self.calculate_next_publication(update_frequency, publication_time, current_time = Time.now)
      DateTime::PublicationScheduler.calculate_next_publication(update_frequency, publication_time, current_time)
    end

    def self.calculate_next_daily_publication(publication_time, current_time)
      DateTime::PublicationScheduler.calculate_next_daily_publication(publication_time, current_time)
    end

    def self.calculate_next_hourly_publication(publication_time, current_time)
      DateTime::PublicationScheduler.calculate_next_hourly_publication(publication_time, current_time)
    end

    def self.publication_time_for_date(date, hour, minute, timezone)
      DateTime::PublicationScheduler.publication_time_for_date(date, hour, minute, timezone)
    end

    def self.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time = Time.now, default_ttl = 3600)
      DateTime::CacheTTLCalculator.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time, default_ttl)
    end

    def self.get_default_ttls
      DateTime::CacheTTLCalculator.get_default_ttls
    end

    def self.determine_fetch_date(metadata)
      DateTime::FetchDateDeterminator.determine_fetch_date(metadata)
    end
  end
end 