module ProviderDateTime
  extend ActiveSupport::Concern
  
  # Calculate next publication time based on update frequency
  # @param update_frequency [Symbol] Update frequency (:daily, :hourly, etc.)
  # @param publication_time [Time] Base publication time
  # @param current_time [Time] Current time
  # @return [Time] Next publication time
  def calculate_next_publication(update_frequency, publication_time, current_time = Time.now)
    Utils::DateTimeHelper.calculate_next_publication(update_frequency, publication_time, current_time)
  end

  # Calculate next daily publication time
  # @param publication_time [Time] Base publication time
  # @param current_time [Time] Current time
  # @param working_days_only [Boolean] Whether to adjust for working days
  # @return [Time] Next publication time
  def calculate_next_daily_publication(publication_time, current_time, working_days_only)
    next_pub = Utils::DateTimeHelper.calculate_next_daily_publication(publication_time, current_time)
    
    # If working days only, adjust for weekends
    Utils::DateTimeHelper.adjust_for_working_days(next_pub, working_days_only)
  end

  # Adjust time for working days if needed
  # @param time [Time] The time to adjust
  # @param working_days_only [Boolean] Whether to adjust for working days
  # @return [Time] Adjusted time
  def adjust_for_working_days(time, working_days_only)
    Utils::DateTimeHelper.adjust_for_working_days(time, working_days_only)
  end

  # Calculate next hourly publication time
  # @param publication_time [Time] Base publication time with minutes
  # @param current_time [Time] Current time
  # @return [Time] Next hourly publication time
  def calculate_next_hourly_publication(publication_time, current_time)
    Utils::DateTimeHelper.calculate_next_hourly_publication(publication_time, current_time)
  end
  
  # Generate the Time object for publication on a given date
  # @param date [Date] The date for which to generate the publication time
  # @param hour [Integer] Publication hour
  # @param minute [Integer] Publication minute
  # @param timezone [String] Publication timezone
  # @return [Time] The publication time for the given date
  def publication_time_for_date(date, hour, minute, timezone)
    Utils::DateTimeHelper.publication_time_for_date(
      date,
      hour,
      minute,
      timezone
    )
  end
  
  # Format publication time for display
  # @param hour [Integer] Publication hour
  # @param minute [Integer] Publication minute
  # @param timezone [String] Publication timezone
  # @return [Time] Formatted publication time
  def format_publication_time(hour, minute, timezone)
    time_string = "#{hour}:#{minute.to_s.rjust(2, '0')} #{timezone}"
    Time.parse(time_string)
  rescue
    nil
  end
  
  # Get the next business day, skipping weekends if necessary
  # @param time [Time, Date] The base time
  # @param working_days_only [Boolean] Whether to adjust for weekends
  # @return [Time] The adjusted time
  def next_business_day(time, working_days_only = true)
    Utils::DateTimeHelper.next_business_day(time, working_days_only)
  end
end 