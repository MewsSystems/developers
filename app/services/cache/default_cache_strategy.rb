require_relative 'cache_strategy'
require_relative '../../errors/exchange_rate_errors'
require_relative '../utils/logging_helper'
require_relative '../utils/date_time_helper'

# Default implementation of cache strategy
# Uses provider metadata to make intelligent caching decisions
class DefaultCacheStrategy < CacheStrategy
  include LoggingHelper

  # Determine the appropriate date to fetch rates for
  # @return [Date] The date to fetch rates for
  def determine_fetch_date
    # Get provider metadata
    metadata = @provider.metadata

    # Use DateTimeHelper to determine fetch date
    Utils::DateTimeHelper.determine_fetch_date(metadata)
  end

  # Check if the cached data for a given date is still fresh
  # @param fetch_date [Date] The date for which data was fetched
  # @return [Boolean] Whether the cached data is still fresh
  def cache_fresh?(fetch_date)
    # Get cache time from repository metadata
    cache_time = @repository.cache_time_for(fetch_date)

    # If we don't have cache time, it's not fresh
    return false unless cache_time

    # Calculate TTL based on provider metadata
    ttl = calculate_cache_ttl

    # Check if cache is still within TTL
    Time.now - cache_time < ttl
  end

  # Calculate cache TTL based on provider metadata
  # @return [Integer] Cache TTL in seconds
  def calculate_cache_ttl
    metadata = @provider.metadata

    # Use default TTL if metadata is missing
    return 3600 unless metadata

    frequency = metadata[:update_frequency]

    case frequency
    when :realtime, :minute
      # For realtime or per-minute updates, cache for a very short time (30 seconds)
      30
    when :hourly
      # For hourly updates, cache for 15 minutes
      15 * 60
    when :daily
      # For daily updates, calculate time until next publication
      next_publication = calculate_next_publication
      if next_publication
        # Cache until next publication time, plus a little margin (5 minutes)
        [(next_publication - Time.now).to_i, 5 * 60].max
      else
        # If we can't determine next publication, use 1 hour as default
        60 * 60
      end
    else
      # Default fallback - 1 hour
      60 * 60
    end
  end

  # Handle errors when fetching rates
  # Overrides base class to add fallback to stale data for certain errors
  # @param error [StandardError] The error that occurred
  # @param fetch_date [Date] The date for which we were fetching rates
  # @return [Array<ExchangeRate>, nil] Fallback rates if available, nil otherwise
  def handle_fetch_error(error, fetch_date)
    # If it's a network or temporary error, try to use stale data
    if error.is_a?(ExchangeRateErrors::NetworkError) ||
       error.is_a?(ExchangeRateErrors::ProviderUnavailableError) ||
       error.is_a?(ExchangeRateErrors::TimeoutError)
      log_warning("Error fetching rates: #{error.message}. Trying to use stale data...")

      # Try to get stale data from repository
      stale_rates = @repository.fetch_for(fetch_date, allow_stale: true)

      if stale_rates
        log_warning("Using stale exchange rate data due to error: #{error.message}")
        return stale_rates
      end
    end

    # For other errors, just raise them
    raise error
  end

  private

  # Calculate next publication time based on provider metadata
  # @return [Time, nil] Next publication time or nil if can't be determined
  def calculate_next_publication
    metadata = @provider.metadata

    # Can't calculate without metadata
    return nil unless metadata

    frequency = metadata[:update_frequency]
    publication_time = metadata[:publication_time]

    # Use DateTimeHelper to calculate next publication
    Utils::DateTimeHelper.calculate_next_publication(
      frequency,
      publication_time,
      Time.now
    )
  end
end