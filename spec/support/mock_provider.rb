require_relative '../../app/providers/base_provider'
require_relative '../../app/domain/exchange_rate'
require_relative '../../app/domain/currency'
require_relative '../../app/errors/exchange_rate_errors'
require_relative '../../app/services/utils/provider_config'

# A mock provider for testing that can simulate various scenarios
class MockProvider < BaseProvider
  attr_accessor :rates, :should_fail
  
  # Initialize a mock provider for testing
  # @param config [Hash] Configuration hash
  def initialize(config = {})
    # Convert string keys to symbols for BaseProvider compatibility
    # But keep the original keys for config hash
    processed_config = config.dup
    
    # Ensure required fields for BaseProvider
    default_config = {
      'base_url' => 'https://api.mock.provider',
      :content_type => 'application/json',
      'base_currency' => 'USD'
    }
    
    # Apply defaults where not provided
    default_config.each do |key, value|
      processed_config[key] ||= value
    end
    
    # Call parent constructor with our config
    super(processed_config)
    
    # Set mock-specific fields
    @rates = []
    @should_fail = config['should_fail'] || false
  end
  
  # Mock implementation of fetch_rates
  # @param date [Date] Date to fetch rates for
  # @param base_currency [String] Base currency code
  # @return [Array<ExchangeRate>] Array of exchange rates
  # @raise [StandardError] If should_fail is true
  def fetch_rates(date = Date.today, base_currency = nil)
    raise StandardError, 'Mock Provider Error' if @should_fail
    @rates
  end
  
  # Set the exchange rates that this mock provider will return
  # @param rates [Array<ExchangeRate>] Array of exchange rates
  def set_rates(rates)
    @rates = rates
  end
  
  # Get metadata about this provider for cache strategy
  # @return [Hash] Provider metadata
  def metadata
    {
      update_frequency: @config&.dig('update_frequency')&.to_sym || :daily,
      publication_time: Time.new(Date.today.year, Date.today.month, Date.today.day, 14, 30, 0),
      source_name: 'Mock Provider',
      base_currency: @base_currency,
      working_days_only: true,
      supports_historical: true
    }
  end
  
  # Set the failure state for this provider
  # @param should_fail [Boolean] Whether API calls should fail
  def toggle_failure(should_fail = true)
    @should_fail = should_fail
  end
end