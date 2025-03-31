require_relative '../../app/providers/base_provider'
require_relative '../../app/domain/exchange_rate'
require_relative '../../app/domain/currency'
require_relative '../../app/services/utils/provider_config'

# Mock provider for testing exchange rates
class ExchangeRateMockProvider < BaseProvider
  def initialize(config = {})
    # Store mock rates before calling super (which will call setup_metadata)
    @mock_rates = config['rates'] || default_rates
    
    # Add base_url to config for tests if not present
    test_config = config.dup
    test_config['base_url'] ||= 'https://mock-exchange.example.com/api'
    
    # Initialize parent
    super(test_config)
  end
  
  # Override to set provider-specific metadata
  def setup_metadata
    @update_frequency = @config[:update_frequency]&.to_sym || :daily
    
    @metadata = {
      source_name: "Mock Exchange Provider",
      base_currency: @base_currency,
      publication_time: format_publication_time,
      update_frequency: @update_frequency,
      supports_historical: false,
      working_days_only: false,
      supported_currencies: @mock_rates ? @mock_rates.keys : []
    }
  end
  
  # Override to implement mock fetch data
  def fetch_data
    # Mock data doesn't need to be fetched
    { data: @mock_rates, format: :mock }
  end
  
  # Override to implement mock parsing
  def parse_data(response)
    # Convert mock rates to ExchangeRate objects
    rates = []
    
    base = Currency.new(@base_currency)
    
    @mock_rates.each do |code, rate_value|
      target = Currency.new(code)
      rates << ExchangeRate.new(
        from: base,
        to: target,
        rate: rate_value,
        date: Date.today
      )
    end
    
    rates
  end
  
  private
  
  # Default exchange rates for testing
  def default_rates
    {
      'EUR' => 0.85,
      'GBP' => 0.75,
      'JPY' => 110.0,
      'CZK' => 22.0,
      'AUD' => 1.35,
      'CAD' => 1.25
    }
  end
end

# RSpec configuration to use the mock provider in tests
RSpec.configure do |config|
  config.before(:each) do
    # Register the mock provider for all tests
    ExchangeRateApplication.configure do |container|
      if defined?(ExchangeRateApplication)
        container.register(:provider, ExchangeRateMockProvider.new)
      end
    end
  end
end 