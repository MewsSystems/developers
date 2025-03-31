class ExchangeRateRepository
  def initialize(storage = {})
    @storage = storage
    @metadata = {}
  end
  
  # Fetch exchange rates for a specific date
  # @param date [Date] The date for which to fetch rates
  # @param allow_stale [Boolean] Whether to allow stale data
  # @return [Array<ExchangeRate>, nil] Array of exchange rates or nil if not found
  def fetch_for(date, allow_stale: false)
    raise NotImplementedError, "Subclasses must implement fetch_for"
  end
  
  # Save exchange rates for a specific date
  # @param date [Date] The date for which to save rates
  # @param rates [Array<ExchangeRate>] The exchange rates to save
  # @return [Array<ExchangeRate>] The saved exchange rates
  def save_for(date, rates)
    raise NotImplementedError, "Subclasses must implement save_for"
  end
  
  # Get caching metadata for a specific date
  # @param date [Date] The date to get metadata for
  # @return [Hash, nil] Metadata hash or nil if not available
  def metadata_for(date)
    @metadata[date]
  end
  
  # Get the time when data for a specific date was last cached
  # @param date [Date] The date to check
  # @return [Time, nil] Cache time or nil if not cached
  def cache_time_for(date)
    @metadata.dig(date, :cached_at)
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
  
  # Check if data exists for a specific date
  # @param date [Date] The date to check
  # @return [Boolean] Whether data exists for the date
  def has_data_for?(date)
    @storage.key?(date)
  end
  
  # Get all cached dates
  # @return [Array<Date>] Array of dates for which data is cached
  def cached_dates
    @storage.keys
  end
end 