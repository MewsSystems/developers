module ProviderConfig
  extend ActiveSupport::Concern

  # Set up provider-specific metadata with default values
  # @param config [Hash] Provider configuration
  # @return [Hash] Provider metadata
  def setup_provider_metadata(provider_name, config)
    source_name = config[:source_display_name] || provider_name
    base_currency = config[:base_currency]
    publication_hour = config[:publication_hour]
    publication_minute = config[:publication_minute]
    publication_timezone = config[:publication_timezone]

    # Format publication time if all required values are present
    publication_time = nil
    if publication_hour && publication_minute && publication_timezone
      publication_time = format_publication_time(publication_hour, publication_minute, publication_timezone)
    end

    Utils::ProviderConfig.build_metadata({
      source_name: source_name,
      base_currency: base_currency,
      publication_time: publication_time
    })
  end

  # Return standard metadata that all providers share
  # @param base_currency [String] Base currency code
  # @param publication_time [Time] Publication time
  # @param working_days_only [Boolean] Whether updates only occur on working days
  # @param supported_currencies [Array<String>] List of supported currencies
  # @return [Hash] Common metadata for all providers
  def standard_metadata(base_currency:, publication_time:, working_days_only: true, supported_currencies: [])
    {
      update_frequency: :daily,
      publication_time: publication_time,
      supports_historical: true,
      base_currency: base_currency,
      working_days_only: working_days_only,
      supported_currencies: supported_currencies
    }
  end

  # Format publication time for display
  # @param hour [Integer] Publication hour
  # @param minute [Integer] Publication minute
  # @param timezone [String] Publication timezone
  # @return [Time] Formatted publication time
  def format_publication_time(hour, minute, timezone)
    return nil unless hour && minute && timezone
    
    # Use Time.use_zone to ensure we parse in the correct timezone
    Time.use_zone('UTC') do
      # First create time in UTC
      base_time = Time.zone.parse("#{hour}:#{minute.to_s.rjust(2, '0')}")
      
      # Convert the timezone offset to seconds
      if timezone.match?(/[+-]\d{1,2}:\d{2}/)
        hours, minutes = timezone.scan(/([+-]\d{1,2}):(\d{2})/)[0]
        offset_seconds = (hours.to_i.abs * 3600 + minutes.to_i * 60) * (hours.start_with?('-') ? -1 : 1)
        # Apply timezone shift
        base_time - offset_seconds
      else
        base_time
      end
    end
  rescue StandardError
    nil
  end
end