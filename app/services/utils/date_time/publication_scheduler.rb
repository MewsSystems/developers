require_relative 'time_constructor'

module Utils
  module DateTime
    # Class to handle publication time scheduling
    class PublicationScheduler
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
        today_pub = TimeConstructor.create_with_components(
          current_time.year, current_time.month, current_time.day,
          publ_hour, publ_min, 0, publ_tz
        )

        if current_time < today_pub
          # Today's publication hasn't happened yet
          today_pub
        else
          # Today's publication has passed, get next day's
          next_day = current_time + 86400 # add 1 day in seconds
          next_day_pub = TimeConstructor.create_with_components(
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
        TimeConstructor.create_with_components(
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
  end
end 