require_relative '../../errors/exchange_rate_errors'
require_relative '../utils/logging_helper'

# Base class for cache strategies
# Defines the interface for all cache strategies to implement
class CacheStrategy
  include LoggingHelper
  
  # Initialize the cache strategy
  # @param provider [BaseProvider] Exchange rate provider
  # @param repository [ExchangeRateRepository] Repository for cached data
  def initialize(provider, repository)
    @provider = provider
    @repository = repository
  end
  
  # Determine the appropriate date to fetch rates for
  # This is an abstract method that should be implemented by subclasses
  # @return [Date] The date to fetch rates for
  def determine_fetch_date
    raise NotImplementedError, "#{self.class.name} must implement determine_fetch_date"
  end
  
  # Check if the cached data for a given date is still fresh
  # This is an abstract method that should be implemented by subclasses
  # @param fetch_date [Date] The date for which data was fetched
  # @return [Boolean] Whether the cached data is still fresh
  def cache_fresh?(fetch_date)
    raise NotImplementedError, "#{self.class.name} must implement cache_fresh?"
  end
  
  # Handle cache refreshing logic
  # @param fetch_date [Date] The date for which to check/refresh cache
  # @param force_refresh [Boolean] Whether to force a refresh regardless of freshness
  # @return [Array<ExchangeRate>, nil] Cached rates if fresh, nil if refresh needed
  def get_cached_rates(fetch_date, force_refresh = false)
    # Get rates from repository
    rates = @repository.fetch_for(fetch_date)
    
    # If no rates are cached or a refresh is forced, return nil to trigger a refresh
    return nil if rates.nil? || force_refresh
    
    # Check if the cached data is still fresh
    if !cache_fresh?(fetch_date)
      # Return nil to trigger a refresh if cache is stale
      return nil
    end
    
    # Return cached rates if they're fresh
    rates
  end
  
  # Calculate cache TTL based on provider metadata
  # @return [Integer] Cache TTL in seconds
  def calculate_cache_ttl
    raise NotImplementedError, "#{self.class.name} must implement calculate_cache_ttl"
  end
  
  # Handle errors when fetching rates
  # @param error [StandardError] The error that occurred
  # @param fetch_date [Date] The date for which we were fetching rates
  # @return [Array<ExchangeRate>, nil] Fallback rates if available, nil otherwise
  def handle_fetch_error(error, fetch_date)
    # By default, re-raise the error
    raise error
  end
end 