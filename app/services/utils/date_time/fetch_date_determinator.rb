require_relative 'business_day_calculator'

module Utils
  module DateTime
    # Class to handle fetching date determination based on metadata
    class FetchDateDeterminator
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
            previous_date = BusinessDayCalculator.previous_business_day(today, working_days_only)

            return previous_date
          end
        end

        # Default: use today's date
        today
      end
    end
  end
end 