require_relative 'exchange_rate_provider_interface'
require_relative '../errors/exchange_rate_errors'
require_relative '../services/utils/provider_helper'
require_relative '../services/utils/provider_config'
require_relative '../services/utils/currency_helper'
require_relative '../services/utils/logging_helper'
require_relative '../services/utils/date_time_helper'
require_relative 'concerns/provider_validation'
require_relative 'concerns/provider_cache'
require_relative 'concerns/provider_operations'
require_relative 'concerns/provider_date_time'
require_relative 'concerns/provider_config'
require_relative 'concerns/provider_data_fetching'

class BaseProvider
  include ExchangeRateProviderInterface
  include LoggingHelper
  include ProviderValidation
  include ProviderCache
  include ProviderOperations
  include ProviderDateTime
  include ProviderConfig
  include ProviderDataFetching

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
    @metadata = setup_provider_metadata(@provider_name, @config)
  end

  # Fetch exchange rates and return an array of ExchangeRate objects.
  # This main public method should be used by clients to get exchange rates.
  def fetch_rates
    # Raise NotImplementedError if called directly on BaseProvider
    ensure_not_base_provider("fetch_rates")

    perform_provider_operation(@provider_name, "fetching rates") do
      # Get the raw data from the source
      response = fetch_data

      # Parse the data using the appropriate adapter
      rates = parse_data(response)

      # Update supported currencies cache based on these rates
      @supported_currencies = extract_supported_currencies(rates)

      rates
    end
  end

  # Fetch data from the source
  # Subclasses should override this if they need custom fetching logic
  def fetch_data
    perform_provider_operation(@provider_name, "fetching data") do
      fetch_http_data(@url, {}, 3, @provider_name)
    end
  end

  # Parse data into exchange rates
  # Subclasses should override this if they need custom parsing logic
  def parse_data(response)
    perform_provider_operation(@provider_name, "parsing data") do
      # Parse the data with appropriate adapter
      rates = parse_data_with_adapter(response, @content_type, @file_extension, @provider_name, @base_currency)

      # Validate rates
      validate_rates(rates, @base_currency, @provider_name)

      rates
    end
  end

  # Return metadata about the provider
  def metadata
    # Raise NotImplementedError if called directly on BaseProvider
    ensure_not_base_provider("metadata")

    # Merge basic metadata with provider-specific metadata
    @metadata.merge(standard_metadata(
      base_currency: @base_currency,
      publication_time: publication_time_for_date(Date.today, @publication_hour, @publication_minute, @publication_timezone),
      working_days_only: true,
      supported_currencies: fetch_supported_currencies
    ))
  end
  
  # Get the list of currency codes supported by this provider
  def fetch_supported_currencies
    # If we already have a cached list of supported currencies, use it
    return @supported_currencies if @supported_currencies

    # Try to get the list without fetching rates if possible
    execute_with_safe_handling do
      rates = fetch_rates
      @supported_currencies = extract_supported_currencies(rates)
    end
  end

  # Get the currencies supported by this provider
  def supported_currencies
    @supported_currencies || fetch_supported_currencies
  end
  
  # For backward compatibility with tests
  # Check if cached data is still fresh
  # @param cached_at [Time] When the data was cached
  # @param current_time [Time] Current time
  # @return [Boolean] Whether the cache is still fresh
  def cache_fresh?(cached_at, current_time = Time.now)
    return false unless cached_at
    
    # Calculate TTL based on metadata if available
    ttl = if respond_to?(:metadata) && metadata
            ttl = cache_ttl(metadata)
          else
            # Default TTL (15 minutes)
            15 * 60
          end
          
    elapsed_time = current_time.to_i - cached_at.to_i
    elapsed_time < ttl
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
end
