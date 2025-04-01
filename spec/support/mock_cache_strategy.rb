require_relative '../../app/services/cache/cache_strategy'
require_relative '../../app/errors/exchange_rate_errors'

# Mock cache strategy for testing
class MockCacheStrategy < CacheStrategy
  attr_accessor :forced_date, :force_fresh, :force_stale, :should_fail, :failure_type
  attr_accessor :stale_data, :use_stale_data_on_error

  def initialize(provider, repository, config = {})
    super(provider, repository)
    @forced_date = config[:forced_date]
    @force_fresh = config[:force_fresh]
    @force_stale = config[:force_stale]
    @should_fail = config[:should_fail] || false
    @failure_type = config[:failure_type] || :cache
    @stale_data = config[:stale_data]
    @use_stale_data_on_error = config[:use_stale_data_on_error]
  end

  # Determine the date to fetch data for
  # @return [Date] The date to fetch data for
  def determine_fetch_date
    return @forced_date if @forced_date

    if @should_fail
      raise ExchangeRateErrors::CacheError.new(
        "Simulated cache error in determine_fetch_date", nil, nil
      )
    end

    Date.today
  end

  # Check if the cached data is still fresh
  # @param fetch_date [Date] The date to check
  # @return [Boolean] Whether the cached data is fresh
  def cache_fresh?(fetch_date)
    return true if @force_fresh
    return false if @force_stale

    if @should_fail
      raise ExchangeRateErrors::CacheError.new(
        "Simulated cache error in cache_fresh?", nil, nil
      )
    end

    # Default - check if data is less than 1 hour old
    cache_time = @repository.cache_time_for(fetch_date)
    return false unless cache_time

    # Fresh if cached less than 1 hour ago
    Time.now - cache_time < 3600
  end

  # Calculate cache TTL based on provider metadata
  # @return [Integer] Cache TTL in seconds
  def calculate_cache_ttl
    if @should_fail
      raise ExchangeRateErrors::CacheError.new(
        "Simulated cache error in calculate_cache_ttl", nil, nil
      )
    end

    # Default TTL of 1 hour
    3600
  end

  # Handle errors when fetching rates
  # @param error [StandardError] The error that occurred
  # @param fetch_date [Date] The date for which we were fetching rates
  # @return [Array<ExchangeRate>, nil] Fallback rates if available, nil otherwise
  def handle_fetch_error(error, fetch_date)
    if @use_stale_data_on_error && @stale_data
      return @stale_data
    end

    # Re-raise the error by default
    raise error
  end
end