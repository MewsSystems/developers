module ExchangeRateProviderInterface
  # Main method to fetch exchange rates
  # @return [Array<ExchangeRate>] Array of exchange rate objects
  def fetch_rates
    raise NotImplementedError, "Providers must implement #fetch_rates"
  end
  
  # Return provider metadata
  # @return [Hash] Provider metadata with keys:
  #   - update_frequency: How often rates are updated (:daily, :hourly, :minute, :realtime)
  #   - publication_time: Time of day when new rates are published (Time object)
  #   - supports_historical: Whether historical data is available (Boolean)
  #   - base_currency: The base currency code (String)
  #   - supported_currencies: Array of supported currency codes (Array<String>)
  #   - working_days_only: Whether updates only occur on working days (Boolean)
  #   - source_name: Display name of the data source (String)
  def metadata
    raise NotImplementedError, "Providers must implement #metadata"
  end
  
  # Get supported currencies
  # @return [Array<String>] List of supported currency codes
  def supported_currencies
    raise NotImplementedError, "Providers must implement #supported_currencies"
  end
end 