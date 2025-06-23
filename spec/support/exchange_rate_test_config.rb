class ExchangeRateTestConfig
  class << self
    attr_accessor :provider_type

    # Get the provider type to use for tests
    # Can be controlled via ENV var or defaults to mock provider
    def provider_type
      @provider_type ||= ENV['EXCHANGE_RATE_TEST_PROVIDER'] || 'mock'
    end

    # Get provider configuration for the selected provider
    def provider_config
      case provider_type
      when 'cnb', 'CNB', 'Cnb'
        {
          'base_url' => ENV['CNB_TEST_URL'] || 'https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt',
          'base_currency' => 'CZK',
          'publication_hour' => 14,
          'publication_minute' => 30,
          'publication_timezone' => '+01:00'
        }
      when 'ecb', 'ECB', 'Ecb'
        {
          'base_url' => ENV['ECB_TEST_URL'] || 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml',
          'base_currency' => 'EUR',
          'publication_hour' => 16,
          'publication_minute' => 0,
          'publication_timezone' => '+01:00'
        }
      when 'mock', 'MOCK', 'Mock'
        # Default mock provider config
        {
          'base_currency' => ENV['MOCK_BASE_CURRENCY'] || 'USD',
          'should_fail' => false,
          'delay_seconds' => 0
        }
      else
        # Default to mock provider if unknown
        {
          'base_currency' => 'USD',
          'should_fail' => false
        }
      end
    end

    # Create a provider instance for tests
    def create_provider
      case provider_type
      when 'cnb', 'CNB', 'Cnb'
        CNBProvider.new(provider_config)
      when 'ecb', 'ECB', 'Ecb'
        ECBProvider.new(provider_config)
      when 'mock', 'MOCK', 'Mock'
        MockProvider.new(provider_config)
      else
        MockProvider.new(provider_config)
      end
    end

    # Create a repository instance for tests
    def create_repository
      MockRepository.new
    end

    # Create a cache strategy for tests
    def create_cache_strategy(provider, repository)
      MockCacheStrategy.new(provider, repository)
    end

    # Create a service instance for tests
    def create_service
      provider = create_provider
      repository = create_repository
      cache_strategy = create_cache_strategy(provider, repository)

      RateService.new(provider, repository, cache_strategy)
    end

    # Create a service with pre-populated mock data
    def create_service_with_data(sample_rates = nil)
      provider = create_provider
      repository = create_repository
      cache_strategy = create_cache_strategy(provider, repository)

      if provider.is_a?(MockProvider) && sample_rates
        provider.set_rates(sample_rates)
      end

      RateService.new(provider, repository, cache_strategy)
    end

    # Reset test configuration
    def reset
      @provider_type = nil
    end

    # Use a specific provider for a test block
    def with_provider(type, &block)
      original_type = @provider_type
      @provider_type = type

      begin
        yield
      ensure
        @provider_type = original_type
      end
    end
  end
end