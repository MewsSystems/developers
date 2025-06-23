require_relative '../../app/services/core/service_container'
require_relative '../../app/services/config/configuration_service'
require_relative '../../app/providers/provider_factory'

# Create the service container as a singleton
module ExchangeRateApplication
  def self.container
    @container ||= ServiceContainer.new
  end

  # Get a service by name
  def self.service(name)
    container.get(name)
  end

  # Get the exchange rate service
  def self.exchange_rate_service
    container.exchange_rate_service
  end

  # Override a service in the container
  # Useful for testing or customization
  def self.register_service(name, instance)
    container.register(name, instance)
  end

  # Configure the application
  # @param block [Proc] Configuration block
  def self.configure
    yield(container) if block_given?
  end
end

# Allow controller access to services
class ApplicationController < ActionController::Base
  protected

  def exchange_rate_service
    ExchangeRateApplication.exchange_rate_service
  end

  def service_container
    ExchangeRateApplication.container
  end

  def get_service(name)
    ExchangeRateApplication.service(name)
  end
end

# Example configuration
ExchangeRateApplication.configure do |container|
  # You can register custom services or override defaults here
  # For example:
  # container.register(:custom_service, CustomService.new)

  # You can also register alternative implementations for testing:
  # if Rails.env.test?
  #   container.register(:provider, MockProvider.new)
  # end
end