require_relative '../../app/errors/exchange_rate_errors'
require_relative '../../app/repositories/exchange_rate_repository'

# Mock repository for testing that can simulate various scenarios
class MockRepository < ExchangeRateRepository
  attr_reader :storage, :metadata
  attr_accessor :should_fail, :failure_type, :error_message
  
  # Initialize a mock repository for testing
  # @param config [Hash] Optional configuration
  def initialize(config = {})
    super
    @storage = {}
    @metadata = {}
    @should_fail = config['should_fail'] || false
    @failure_type = config['failure_type'] || :storage
    @error_message = config['error_message'] || "Simulated repository error"
    @rates = {}
    @cache_times = {}
  end
  
  # Fetch exchange rates for a specific date
  # @param date [Date] The date to fetch rates for
  # @param allow_stale [Boolean] Whether to allow stale data
  # @return [Array<ExchangeRate>, nil] Exchange rates for the date or nil if not cached
  def fetch_for(date, allow_stale: false)
    if @should_fail
      case @failure_type
      when :storage, 'storage'
        raise ExchangeRateErrors::StorageError.new(
          "Simulated storage error: #{@error_message}", nil, nil
        )
      when :stale, 'stale'
        # Return nil if stale data not allowed
        return nil unless allow_stale
        # Otherwise, return data with a warning
        return @storage[date]
      else
        raise ExchangeRateErrors::RepositoryError.new(
          "Simulated repository error (#{@failure_type}): #{@error_message}", nil, nil
        )
      end
    end
    
    @rates[date]
  end
  
  # Save exchange rates for a specific date
  # @param date [Date] The date to save rates for
  # @param rates [Array<ExchangeRate>] The rates to save
  # @return [Array<ExchangeRate>] The saved rates
  def save_for(date, rates)
    if @should_fail
      case @failure_type
      when :storage, 'storage'
        raise ExchangeRateErrors::StorageError.new(
          "Simulated storage error: #{@error_message}", nil, nil
        )
      else
        raise ExchangeRateErrors::RepositoryError.new(
          "Simulated repository error (#{@failure_type}): #{@error_message}", nil, nil
        )
      end
    end
    
    @rates[date] = rates
    @cache_times[date] = Time.now
    rates
  end
  
  # Get caching metadata for a specific date
  # @param date [Date] The date to get metadata for
  # @return [Hash, nil] Metadata hash or nil if not available
  def metadata_for(date)
    @metadata[date]
  end
  
  # Get the cache time for a specific date
  # @param date [Date] The date to get cache time for
  # @return [Time, nil] The time when rates were cached or nil if not cached
  def cache_time_for(date)
    @cache_times[date]
  end
  
  # Clear cached data for a specific date
  # @param date [Date] The date to clear
  def clear(date)
    @storage.delete(date)
    @metadata.delete(date)
  end
  
  # Clear all cached data
  def clear_all
    @storage.clear
    @metadata.clear
  end
  
  # Populate with sample data for testing
  def populate_with_sample_data(rates, date = Date.today)
    @storage[date] = rates
    @metadata[date] = { cached_at: Time.now - 60 } # 1 minute ago
  end
  
  # Set stale data for testing cache freshness
  def set_stale_data(rates, date = Date.today, hours_old = 24)
    @storage[date] = rates
    @metadata[date] = { cached_at: Time.now - (hours_old * 3600) }
  end
  
  # Toggle failure mode
  def toggle_failure(should_fail = true, type = :storage, message = "Simulated repository error")
    @should_fail = should_fail
    @failure_type = type
    @error_message = message
  end
end 