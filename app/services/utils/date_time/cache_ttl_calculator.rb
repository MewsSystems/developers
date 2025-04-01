require_relative 'publication_scheduler'

module Utils
  module DateTime
    # Class to handle cache TTL calculations
    class CacheTTLCalculator
      # Calculate cache TTL until next publication
      # @param update_frequency [Symbol] Update frequency
      # @param publication_time [Time] Publication time
      # @param current_time [Time] Current time
      # @param default_ttl [Integer] Default TTL if calculation fails
      # @return [Integer] TTL in seconds
      def self.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time = Time.now,
                                                  default_ttl = 3600)
        next_pub = PublicationScheduler.calculate_next_publication(update_frequency, publication_time, current_time)
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
  end
end 