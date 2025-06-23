require_relative '../config/configuration_service'
require_relative '../cache/default_cache_strategy'
require_relative '../exchange_rate/rate_service'
require_relative '../../repositories/exchange_rate_repository'
require_relative '../../repositories/in_memory_exchange_rate_repository'
require_relative '../../repositories/redis_exchange_rate_repository'

class ServiceContainer
  def initialize
    @services = {}
    @factories = {}
    register_defaults
  end

  # Register a service instance
  # @param name [Symbol] Service name
  # @param instance [Object] Service instance
  # @return [Object] The registered instance
  def register(name, instance)
    @services[name.to_sym] = instance
  end

  # Register a factory method for a service
  # @param name [Symbol] Service name
  # @param block [Proc] Factory method
  def register_factory(name, &block)
    @factories[name.to_sym] = block
  end

  # Get a service by name
  # @param name [Symbol] Service name
  # @return [Object] Service instance
  def get(name)
    name = name.to_sym

    # Return the service if it's already created
    return @services[name] if @services.key?(name)

    # Create the service if a factory is registered
    if @factories.key?(name)
      @services[name] = @factories[name].call(self)
      return @services[name]
    end

    raise ArgumentError, "No service or factory registered for #{name}"
  end

  # Check if a service is registered
  # @param name [Symbol] Service name
  # @return [Boolean] Whether the service is registered
  def has?(name)
    name = name.to_sym
    @services.key?(name) || @factories.key?(name)
  end

  # Get the exchange rate service
  # @return [ExchangeRateService] Exchange rate service
  def exchange_rate_service
    get(:exchange_rate_service)
  end

  # Get the provider factory
  # @return [ProviderFactory] Provider factory
  def provider_factory
    get(:provider_factory)
  end

  # Get the configuration service
  # @return [ConfigurationService] Configuration service
  def configuration
    get(:configuration)
  end

  private

  # Register default services and factories
  def register_defaults
    # Register the configuration service
    register_factory(:configuration) do |container|
      config_path = defined?(Rails) ? Rails.root.join('config/exchange_rates.yml') : nil
      ConfigurationService.new(config_path)
    end

    # Register the provider factory
    register_factory(:provider_factory) do |container|
      ProviderFactory
    end

    # Register the provider
    register_factory(:provider) do |container|
      config = container.configuration
      container.provider_factory.create(
        config.provider_type,
        config.provider_config
      )
    end

    # Register the repository
    register_factory(:repository) do |container|
      RedisExchangeRateRepository.new
    end

    # Register the cache strategy
    register_factory(:cache_strategy) do |container|
      DefaultCacheStrategy.new(container.get(:provider), container.get(:repository))
    end

    # Register the exchange rate service
    register_factory(:exchange_rate_service) do |container|
      RateService.new(
        container.get(:provider),
        container.get(:repository),
        container.get(:cache_strategy)
      )
    end
  end
end