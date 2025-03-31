require_relative '../../providers/exchange_rate_provider_interface'
require_relative '../../errors/exchange_rate_errors'
require_relative '../cache/default_cache_strategy'
require_relative '../utils/logging_helper'
require_relative '../utils/provider_helper'
require_relative '../utils/currency_helper'

class RateService
  include LoggingHelper
  
  class CurrencyWarning < StandardError; end

  # Initialize with a provider, repository, and cache strategy
  # @param provider [ExchangeRateProviderInterface] Exchange rate provider
  # @param repository [ExchangeRateRepository] Repository for caching
  # @param cache_strategy [CacheStrategy] Cache strategy
  def initialize(provider, repository, cache_strategy = nil)
    # Verify that provider implements the required interface
    unless provider.respond_to?(:fetch_rates) && provider.respond_to?(:metadata)
      raise ExchangeRateErrors::InvalidConfigurationError.new(
        "Provider must implement ExchangeRateProviderInterface",
        nil, nil, { provider_class: provider.class.name }
      )
    end
    
    @provider = provider
    @repository = repository
    @cache_strategy = cache_strategy || DefaultCacheStrategy.new(@provider, @repository)
    # For tracking unavailable currency warnings
    @unavailable_currencies = {}
    @provider_name = Utils::ProviderHelper.provider_name_without_suffix(@provider)
  end

  # Get exchange rates, optionally filtered by currency
  # @param currencies [Array<String>, nil] Currency codes to filter by, or nil for all
  # @param force_refresh [Boolean] Whether to force a cache refresh
  # @return [Array<ExchangeRate>] Array of exchange rates
  # @raise [ExchangeRateErrors::Error] If an error occurs
  def get_rates(currencies = nil, force_refresh = false)
    begin
      # Get appropriate date from cache strategy
      fetch_date = @cache_strategy.determine_fetch_date
      
      # Try to get rates from cache
      rates = @cache_strategy.get_cached_rates(fetch_date, force_refresh)
      
      # If no cached rates or cache is stale, fetch from provider
      unless rates
        begin
          # Fetch fresh rates from provider
          rates = @provider.fetch_rates
          
          # Save to repository
          @repository.save_for(fetch_date, rates)
        rescue => e
          # Let the cache strategy handle the error
          # It might provide stale data or re-raise the error
          rates = @cache_strategy.handle_fetch_error(e, fetch_date)
        end
      end

      # Filter by currencies if specified
      if currencies&.any?
        # Validate requested currencies
        validate_requested_currencies(currencies, rates)
        
        # Check for requested currencies not available from this provider
        upcase_currencies = currencies.map(&:upcase)
        check_currency_availability(upcase_currencies, rates)
        
        # Filter to only include requested currencies
        codes = upcase_currencies.to_set
        rates = rates.select { |rate| codes.include?(rate.to.code) }
      end

      rates
    rescue ExchangeRateErrors::Error => e
      # Re-raise ExchangeRateErrors
      raise e
    rescue => e
      # Wrap unexpected errors
      raise ExchangeRateErrors::ServiceError.new(
        "Unexpected error in exchange rate service: #{e.message}",
        e, @provider_name
      )
    end
  end
  
  # Get a specific exchange rate between two currencies
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate] Exchange rate
  # @raise [ExchangeRateErrors::CurrencyNotSupportedError] If currencies not supported
  # @raise [ExchangeRateErrors::NoExchangeRateAvailableError] If no rate available
  def get_rate(from_currency, to_currency)
    from_currency = normalize_currency_code(from_currency)
    to_currency = normalize_currency_code(to_currency)
    
    # Get rates for both currencies
    rates = get_rates([from_currency, to_currency])
    
    # Try different strategies to find or calculate the exchange rate
    exchange_rate = find_direct_rate(rates, from_currency, to_currency) ||
                    calculate_inverse_rate(rates, from_currency, to_currency) ||
                    calculate_cross_rate(rates, from_currency, to_currency)
    
    # If no rate available, raise error
    unless exchange_rate
      raise_no_exchange_rate_error(from_currency, to_currency)
    end
    
    exchange_rate
  end
  
  # Convert an amount from one currency to another
  # @param amount [Numeric] Amount to convert
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [Hash] Hash with :amount, :from, :to, and :rate
  def convert(amount, from_currency, to_currency)
    amount = amount.to_f
    
    # Validate amount
    if amount <= 0
      raise ExchangeRateErrors::ValidationError.new(
        "Amount must be positive: #{amount}",
        nil, nil, { amount: amount }
      )
    end
    
    # Get the exchange rate
    rate = get_rate(from_currency, to_currency)
    
    # Calculate converted amount
    converted_amount = amount * rate.rate
    
    {
      amount: amount,
      from: from_currency,
      to: to_currency,
      rate: rate.rate,
      converted_amount: converted_amount,
      date: rate.date
    }
  end
  
  # Returns a hash of currencies that have been requested but were unavailable
  # The keys are currency codes, values are the timestamp when first requested
  # @return [Hash] Hash of currency codes to timestamps
  def unavailable_currencies
    @unavailable_currencies.dup.freeze
  end
  
  # Access to the provider (useful for tests and debugging)
  attr_reader :provider
  
  private
  
  # Validate that requested currencies are supported
  # @param currencies [Array<String>] Requested currencies
  # @param available_rates [Array<ExchangeRate>] Available rates
  # @raise [ExchangeRateErrors::CurrencyNotSupportedError] If currencies not supported
  def validate_requested_currencies(currencies, available_rates)
    # Ensure all currencies are valid
    begin
      normalized_currencies = Utils::CurrencyHelper.normalize_codes(currencies)
    rescue Utils::CurrencyHelper::InvalidCurrencyCodeError => e
      raise ExchangeRateErrors::ValidationError.new(e.message)
    end
    
    # Check if provider supports the base currency conversion
    supported_currencies = @provider.supported_currencies
    
    # If provider doesn't list supported currencies, extract from rates
    if supported_currencies.nil? || supported_currencies.empty?
      supported_currencies = Utils::CurrencyHelper.extract_currency_codes(available_rates)
    end
    
    # Find unsupported currencies
    unsupported = normalized_currencies.reject do |code|
      supported_currencies.include?(code)
    end
    
    if unsupported.any?
      raise ExchangeRateErrors::CurrencyNotSupportedError.new(
        "Currencies not supported by #{@provider_name}: #{unsupported.join(', ')}",
        nil, @provider_name,
        { 
          unsupported: unsupported,
          supported: supported_currencies
        }
      )
    end
  end
  
  # Check if any requested currencies are not available from the current provider
  # Logs warnings for unavailable currencies to avoid repeated unnecessary requests
  # @param requested_currencies [Array<String>] Requested currency codes
  # @param available_rates [Array<ExchangeRate>] Available rates
  def check_currency_availability(requested_currencies, available_rates)
    available_codes = available_rates.map { |rate| rate.to.code }.to_set
    
    # Find currencies that are now unavailable
    unavailable_now = requested_currencies.reject do |code|
      available_codes.include?(code)
    end
    
    return if unavailable_now.empty?
    
    # Record time of first request for unavailable currencies
    now = Time.now.to_i
    unavailable_now.each do |code|
      @unavailable_currencies[code] ||= now
      log_unavailable_currency(code, available_codes)
    end
  end
  
  # Log a warning about an unavailable currency (maintained for test compatibility)
  # @param code [String] Unavailable currency code
  # @param available_codes [Set<String>] Available currency codes
  def log_unavailable_currency(code, available_codes)
    message = "Currency '#{code}' was requested but is not available from #{@provider_name}. " \
              "Available currencies: #{available_codes.to_a.sort.join(', ')}"
              
    log_warning(message)
  end
  
  # Normalize a currency code by converting to string, trimming whitespace, and upcasing
  # @param code [String] Currency code to normalize
  # @return [String] Normalized currency code
  def normalize_currency_code(code)
    begin
      Utils::CurrencyHelper.normalize_code(code)
    rescue Utils::CurrencyHelper::InvalidCurrencyCodeError => e
      raise ExchangeRateErrors::ValidationError.new(e.message)
    end
  end
  
  # Find a direct exchange rate between two currencies
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Direct exchange rate or nil if not found
  def find_direct_rate(rates, from_currency, to_currency)
    rates.find { |rate| 
      rate.from.code == from_currency && rate.to.code == to_currency 
    }
  end
  
  # Calculate inverse exchange rate between two currencies
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Inverse exchange rate or nil if not possible
  def calculate_inverse_rate(rates, from_currency, to_currency)
    inverse_rate = rates.find { |rate| 
      rate.from.code == to_currency && rate.to.code == from_currency 
    }
    
    return nil unless inverse_rate
    
    # Return the inverse of the rate
    ExchangeRate.new(
      from: inverse_rate.to,
      to: inverse_rate.from,
      rate: 1.0 / inverse_rate.rate,
      date: inverse_rate.date
    )
  end
  
  # Calculate cross exchange rate via the base currency
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Cross exchange rate or nil if not possible
  def calculate_cross_rate(rates, from_currency, to_currency)
    base_currency = @provider.metadata[:base_currency]
    
    # Find rates to convert via base currency
    from_to_base = rates.find { |rate| 
      rate.from.code == base_currency && rate.to.code == from_currency 
    }
    
    base_to_to = rates.find { |rate| 
      rate.from.code == base_currency && rate.to.code == to_currency 
    }
    
    return nil unless from_to_base && base_to_to
    
    # Calculate cross rate
    cross_rate = base_to_to.rate / from_to_base.rate
    
    ExchangeRate.new(
      from: Currency.new(from_currency),
      to: Currency.new(to_currency),
      rate: cross_rate,
      date: base_to_to.date
    )
  end
  
  # Raise a standardized error when no exchange rate is available
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @raise [ExchangeRateErrors::NoExchangeRateAvailableError] No exchange rate available error
  def raise_no_exchange_rate_error(from_currency, to_currency)
    raise ExchangeRateErrors::NoExchangeRateAvailableError.new(
      "No exchange rate available for #{from_currency} to #{to_currency}",
      nil, @provider_name,
      { from: from_currency, to: to_currency }
    )
  end
end 