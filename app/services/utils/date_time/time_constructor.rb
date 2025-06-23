module Utils
  module DateTime
    # Class to handle time object creation and manipulation
    class TimeConstructor
      # Create a Time object with the given components
      # @param year [Integer] Year
      # @param month [Integer] Month
      # @param day [Integer] Day
      # @param hour [Integer] Hour
      # @param min [Integer] Minute
      # @param sec [Integer] Second
      # @param timezone [String] Timezone offset string
      # @return [Time] Time object with the given components
      def self.create_with_components(year, month, day, hour, min, sec, timezone)
        Time.new(year, month, day, hour, min, sec, timezone)
      end
    end
  end
end