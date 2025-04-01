require_relative 'time_constructor'

module Utils
  module DateTime
    # Class to handle business day calculations
    class BusinessDayCalculator
      # Get the next business day, skipping weekends if necessary
      # @param time [Time, Date] The base time
      # @param working_days_only [Boolean] Whether to adjust for weekends
      # @return [Time, Date] The adjusted time
      def self.next_business_day(time, working_days_only: true)
        return time unless working_days_only

        # Handle Date objects specifically
        if time.is_a?(Date)
          date = time
          # Skip weekends
          date += 2 if date.saturday? # Saturday to Monday
          date += 1 if date.sunday?   # Sunday to Monday
          return date
        end

        # Get day of week (0 = Sunday, 6 = Saturday)
        wday = time.wday

        case wday
        when 0 # Sunday - move to Monday (+1 day)
          TimeConstructor.create_with_components(
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
            TimeConstructor.create_with_components(
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
      def self.previous_business_day(date, working_days_only: true)
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
      def self.adjust_for_working_days(time, working_days_only: true)
        working_days_only ? next_business_day(time, working_days_only: working_days_only) : time
      end
    end
  end
end