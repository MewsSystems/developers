# Provider Template

This document serves as a guide for implementing new exchange rate providers.

## Getting Started

To create a new provider:

1. Create a new file named `your_provider_name_provider.rb` in the `app/providers` directory
2. Subclass `BaseProvider`
3. Implement the required methods and configuration
4. Add your provider to `ProviderFactory.PROVIDER_CLASSES` mapping

## Implementation Reference

### Required Methods

- `initialize(config)`: Configure the provider with appropriate defaults
- `setup_metadata`: Override to set provider-specific metadata

### Optional Methods to Override

- `fetch_data`: If custom data fetching is needed
- `parse_data`: If custom parsing or validation is needed

## Example Implementation

```ruby
require_relative 'base_provider'
require_relative '../adapters/adapter_factory'
require_relative '../fetchers/http_fetcher'
require_relative '../errors/exchange_rate_errors'
require_relative '../services/utils/provider_config'

class YourProviderNameProvider < BaseProvider
  def initialize(config = {})
    # Use configuration helper with provider-specific defaults
    config_with_defaults = Utils::ProviderConfig.configure('YourProvider', config, {
      base_currency: 'USD',
      publication_hour: 16,
      publication_minute: 0,
      publication_timezone: '+00:00', # UTC
      required_fields: ['base_url']
    })
    
    super(config_with_defaults)
  end

  # Set provider-specific metadata
  def setup_metadata
    @metadata = Utils::ProviderConfig.build_metadata({
      source_name: 'Your Provider Name',
      base_currency: @base_currency,
      publication_time: format_publication_time
    })
  end
  
  # -----------------------------------------------------
  # EXAMPLES OF OPTIONAL METHOD OVERRIDES
  # -----------------------------------------------------
  
  # Example of custom data fetching (if needed)
  def fetch_data
    # Custom implementation for fetching data
    # For example, using a different strategy or headers
    custom_headers = { 'X-API-Key' => @config[:api_key] }
    Utils::ProviderHelper.with_provider_error_handling(@provider_name, "fetching data") do
      HttpFetcher.fetch(@url, custom_headers, 3, @provider_name)
    end
  end
  
  # Example of custom data parsing (if needed)
  def parse_data(response)
    # Call the parent implementation first
    rates = super(response)
    
    # Add provider-specific validation
    validate_rates(rates)
    
    rates
  end
  
  private
  
  # Example of provider-specific validation
  def validate_rates(rates)
    if rates.empty?
      raise ExchangeRateErrors::ValidationError.new(
        "No rates found", nil, @provider_name
      )
    end
  end
end
```

## Adding to ProviderFactory

After creating your provider, add it to the `PROVIDER_CLASSES` hash in `app/providers/provider_factory.rb`:

```ruby
PROVIDER_CLASSES = {
  'cnb' => 'CNBProvider',
  'ecb' => 'ECBProvider',
  'your_provider' => 'YourProviderNameProvider'
}.freeze
``` 