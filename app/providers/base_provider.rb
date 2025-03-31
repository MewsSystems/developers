require_relative 'exchange_rate_provider_interface'
require_relative '../errors/exchange_rate_errors'
require_relative '../services/utils/provider_helper'
require_relative '../services/utils/provider_config'
require_relative '../services/utils/currency_helper'
require_relative '../services/utils/logging_helper'
require_relative '../services/utils/date_time_helper'

class BaseProvider
  include ExchangeRateProviderInterface
  include LoggingHelper
  
  attr_reader :metadata

  def initialize(config = {})
    @provider_name = Utils::ProviderHelper.provider_name_without_suffix(self)
    
    # Apply configuration using provider defaults
    @config = Utils::ProviderConfig.configure(@provider_name, config)
    
    @base_currency = @config[:base_currency]
    @publication_hour = @config[:publication_hour]
    @publication_minute = @config[:publication_minute]
    @publication_timezone = @config[:publication_timezone]
    @content_type = @config[:content_type]
    
    # Fix: ensure url is set from base_url in the config
    @url = @config[:base_url] || @config['base_url']
    
    # Extract file extension from URL for adapter selection if needed
    @file_extension = @config[:file_extension] || (@url ? File.extname(@url) : nil)
    
    # Cache for supported currencies to avoid repeated fetches
    @supported_currencies = nil
    
    # Initialize provider-specific metadata
    setup_metadata
  end

  # Set up provider-specific metadata with default values
  # This can be overridden by subclasses to provide source-specific details
  def setup_metadata
    source_name = @config[:source_display_name] || @provider_name
    
    @metadata = Utils::ProviderConfig.build_metadata({
      source_name: source_name,
      base_currency: @base_currency,
      publication_time: format_publication_time
    })
  end

  # Format publication time for display
  def format_publication_time
    time_string = "#{@publication_hour}:#{@publication_minute.to_s.rjust(2, '0')} #{@publication_timezone}"
    Time.parse(time_string)
  rescue
    nil
  end

  # Fetch exchange rates and return an array of ExchangeRate objects.
  # This main public method should be used by clients to get exchange rates.
  def fetch_rates
    # Raise NotImplementedError if called directly on BaseProvider
    ensure_not_base_provider("fetch_rates")
    
    perform_provider_operation("fetching rates") do
      # Get the raw data from the source
      response = fetch_data
      
      # Parse the data using the appropriate adapter
      rates = parse_data(response)
      
      # Update supported currencies cache based on these rates
      @supported_currencies = Utils::CurrencyHelper.extract_currency_codes(rates)
      
      rates
    end
  end
  
  # Fetch data from the source
  # Subclasses should override this if they need custom fetching logic
  def fetch_data
    perform_provider_operation("fetching data") do
      HttpFetcher.fetch(@url, {}, 3, @provider_name)
    end
  end
  
  # Parse data into exchange rates
  # Subclasses should override this if they need custom parsing logic
  def parse_data(response)
    perform_provider_operation("parsing data") do
      # Figure out the right adapter to use
      content_type = response[:content_type] || @content_type
      file_extension = response[:file_extension] || @file_extension
      
      adapter = if content_type
                  AdapterFactory.for_content_type(@provider_name, content_type)
                elsif file_extension
                  AdapterFactory.for_file_extension(@provider_name, file_extension)
                else
                  # Try to auto-detect from content
                  AdapterFactory.for_content(@provider_name, response[:data])
                end
      
      # Parse the data
      rates = adapter.parse(response[:data], @base_currency)
      
      # Validate rates
      validate_rates(rates)
      
      rates
    end
  end
  
  # Centralized error handling for provider operations
  # @param operation_name [String] Name of the operation being performed
  # @yield Block to execute with error handling
  # @return [Object] Result of the block
  # @raise [ExchangeRateErrors::Error] If an error occurs
  def perform_provider_operation(operation_name)
    Utils::ProviderHelper.with_provider_error_handling(@provider_name, operation_name) do
      yield
    end
  end
  
  # Return metadata about the provider including:
  # - update_frequency: How often rates are updated (:daily, :hourly, :minute, :realtime, etc.)
  # - publication_time: Time of day when new rates are published
  # - supports_historical: Whether the provider supports historical data
  # - base_currency: The base currency for returned rates
  # - supported_currencies: Array of supported currency codes
  # - working_days_only: Whether the provider only updates on working days
  def metadata
    # Raise NotImplementedError if called directly on BaseProvider
    ensure_not_base_provider("metadata")
    
    # Merge basic metadata with provider-specific metadata
    @metadata.merge(standard_metadata)
  end
  
  # Return basic metadata that all providers share
  # @return [Hash] Common metadata for all providers
  def standard_metadata
    {
      update_frequency: :daily,
      publication_time: publication_time_for_date(Date.today),
      supports_historical: true,
      base_currency: @base_currency,
      working_days_only: true,
      supported_currencies: fetch_supported_currencies
    }
  end
  
  # Validate basic aspects of exchange rates that apply to all providers
  # @param rates [Array<ExchangeRate>] Exchange rates to validate
  # @raise [ExchangeRateErrors::ValidationError] If rates are invalid
  def validate_rates(rates)
    validate_rates_not_empty(rates)
    validate_base_currency(rates)
    
    # Call provider-specific validator method if it exists
    if respond_to?(:validate_provider_specific_rates, true)
      validate_provider_specific_rates(rates)
    end
  end
  
  # Check if rates array is empty
  # @param rates [Array<ExchangeRate>] Exchange rates to validate
  # @raise [ExchangeRateErrors::ValidationError] If rates array is empty
  def validate_rates_not_empty(rates)
    if rates.empty?
      raise_validation_error("No exchange rates found in #{@provider_name} data")
    end
  end
  
  # Verify all rates have the correct base currency
  # @param rates [Array<ExchangeRate>] Exchange rates to validate
  # @raise [ExchangeRateErrors::ValidationError] If base currency is incorrect
  def validate_base_currency(rates)
    incorrect_base = rates.find { |rate| rate.from.code != @base_currency }
    if incorrect_base
      raise_validation_error(
        "Unexpected base currency: expected #{@base_currency}, got #{incorrect_base.from.code}",
        { expected: @base_currency, actual: incorrect_base.from.code }
      )
    end
  end
  
  # Get the list of currency codes supported by this provider
  # This is a convenience method to avoid fetching full rates when only
  # the available currencies are needed
  def fetch_supported_currencies
    # If we already have a cached list of supported currencies, use it
    return @supported_currencies if @supported_currencies
    
    # Try to get the list without fetching rates if possible
    execute_with_safe_handling do
      rates = fetch_rates
      @supported_currencies = Utils::CurrencyHelper.extract_currency_codes(rates)
    end
  end
  
  # Helper method to safely execute a block and return a default value if it fails
  # @param default [Object] Default value to return on error
  # @yield Block to execute
  # @return [Object] Result of block or default
  def execute_with_safe_handling(default = [])
    yield
  rescue => e
    # If the operation fails, log a warning and return the default
    log_message("Operation failed: #{e.message}", :warn, self.class.name)
    default
  end
  
  # Get the currencies supported by this provider
  # This method exists for compatibility with the interface
  def supported_currencies
    @supported_currencies || fetch_supported_currencies
  end
  
  # Check if a specific currency is supported by this provider
  # @param code [String] Currency code to check
  # @return [Boolean] Whether the currency is supported
  def supports_currency?(code)
    return false unless code
    code = code.to_s.strip.upcase
    supported_currencies.include?(code)
  end
  
  # Calculate cache TTL based on provider metadata
  # @param current_time [Time] Current time (defaults to Time.now)
  # @return [Integer] Cache TTL in seconds
  def cache_ttl(current_time = Time.now)
    metadata = self.metadata
    update_frequency = metadata[:update_frequency]
    
    # Default TTLs by update frequency
    ttl_defaults = Utils::DateTimeHelper.get_default_ttls
    
    # Use default TTL or calculate based on next publication
    if update_frequency == :daily && metadata[:publication_time]
      Utils::DateTimeHelper.calculate_ttl_until_next_publication(
        update_frequency, 
        metadata[:publication_time], 
        current_time, 
        ttl_defaults[:daily]
      )
    else
      # Use default TTL for this frequency or fall back to hourly (3600)
      ttl_defaults[update_frequency] || 3600
    end
  end
  
  # Check if cached data is still fresh
  # @param cached_at [Time] When the data was cached
  # @param current_time [Time] Current time
  # @return [Boolean] Whether the cache is still fresh
  def cache_fresh?(cached_at, current_time = Time.now)
    return false unless cached_at
    (current_time - cached_at) < cache_ttl(current_time)
  end
  
  # Calculate next publication time based on update frequency
  # @param update_frequency [Symbol] Update frequency (:daily, :hourly, etc.)
  # @param publication_time [Time] Base publication time
  # @param current_time [Time] Current time
  # @return [Time] Next publication time
  def calculate_next_publication(update_frequency = nil, publication_time = nil, current_time = Time.now)
    # Default to metadata values if not provided
    update_frequency ||= metadata[:update_frequency]
    publication_time ||= metadata[:publication_time]
    
    Utils::DateTimeHelper.calculate_next_publication(update_frequency, publication_time, current_time)
  end
  
  # Calculate next daily publication time
  # @param publication_time [Time] Base publication time
  # @param current_time [Time] Current time
  # @return [Time] Next publication time
  def calculate_next_daily_publication(publication_time, current_time)
    next_pub = Utils::DateTimeHelper.calculate_next_daily_publication(publication_time, current_time)
    working_days_only = metadata[:working_days_only]
    
    # If working days only, adjust for weekends
    Utils::DateTimeHelper.adjust_for_working_days(next_pub, working_days_only)
  end
  
  # Adjust time for working days if needed
  # @param time [Time] The time to adjust
  # @return [Time] Adjusted time
  def adjust_for_working_days(time)
    working_days_only = metadata[:working_days_only]
    Utils::DateTimeHelper.adjust_for_working_days(time, working_days_only)
  end
  
  # Calculate next hourly publication time
  # @param publication_time [Time] Base publication time with minutes
  # @param current_time [Time] Current time
  # @return [Time] Next hourly publication time
  def calculate_next_hourly_publication(publication_time, current_time)
    Utils::DateTimeHelper.calculate_next_hourly_publication(publication_time, current_time)
  end
  
  # Helper method to create a Time object with the given components
  # @param year [Integer] Year
  # @param month [Integer] Month
  # @param day [Integer] Day
  # @param hour [Integer] Hour
  # @param min [Integer] Minute
  # @param sec [Integer] Second
  # @param timezone [String] Timezone offset string
  # @return [Time] Time object with the given components
  def create_time_with_components(year, month, day, hour, min, sec, timezone)
    Utils::DateTimeHelper.create_time_with_components(year, month, day, hour, min, sec, timezone)
  end
  
  # Get the next business day, skipping weekends if necessary
  # @param time [Time, Date] The base time
  # @param working_days_only [Boolean] Whether to adjust for weekends
  # @return [Time] The adjusted time
  def next_business_day(time, working_days_only = true)
    Utils::DateTimeHelper.next_business_day(time, working_days_only)
  end
  
  protected
  
  # Ensure method is not called directly on BaseProvider
  # @param method_name [String] The name of the method being called
  # @raise [NotImplementedError] If called on BaseProvider
  def ensure_not_base_provider(method_name)
    if self.class == BaseProvider
      raise NotImplementedError, "Subclasses must implement #{method_name}"
    end
  end
  
  # Generate the Time object for publication on a given date
  # @param date [Date] The date for which to generate the publication time
  # @return [Time] The publication time for the given date
  def publication_time_for_date(date)
    Utils::DateTimeHelper.publication_time_for_date(
      date, 
      @publication_hour, 
      @publication_minute, 
      @publication_timezone
    )
  end
  
  # Create a validation error with provider context
  # @param message [String] Error message
  # @param context [Hash] Additional context for the error
  # @raise [ExchangeRateErrors::ValidationError] Validation error
  def raise_validation_error(message, context = {})
    raise ExchangeRateErrors::ValidationError.new(
      message,
      nil, @provider_name, context
    )
  end
end 