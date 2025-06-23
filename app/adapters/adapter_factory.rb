require_relative 'formats/txt_adapter'
require_relative 'formats/xml_adapter'
require_relative 'formats/json_adapter'
require_relative 'base/base_adapter'
require_relative 'providers/cnb/cnb_text_adapter'
require_relative 'providers/cnb/cnb_xml_adapter'
require_relative 'providers/cnb/cnb_json_adapter'
require_relative '../errors/exchange_rate_errors'
require_relative '../services/utils/format_helper'
require_relative 'registry/adapter_registry'
require_relative 'registry/registry_initializer'
require_relative 'services/adapter_creator'

# Factory for creating adapters
class AdapterFactory
  # List of supported providers
  DEFAULT_PROVIDERS = ['CNB', 'Test', 'ECB'].freeze

  class << self
    # Keep track of added providers
    attr_reader :additional_providers

    def supported_providers
      DEFAULT_PROVIDERS + additional_providers
    end
  end

  # Initialize additional providers
  @additional_providers = []

  # Initialize the adapter registry with default adapters
  def self.initialize_registry
    Adapters::Registry::RegistryInitializer.initialize_registry(supported_providers)
  end

  # Create an adapter based on file extension
  # @param provider_name [String] Name of the provider
  # @param file_extension [String] File extension
  # @return [BaseAdapter] Appropriate adapter for the file extension
  # @raise [ExchangeRateErrors::UnsupportedFormatError] If no adapter supports the file extension
  def self.for_file_extension(provider_name, file_extension)
    ensure_provider_registered(provider_name)
    Adapters::Services::AdapterCreator.for_file_extension(provider_name, file_extension)
  end

  # Create an adapter by content type
  # @param provider_name [String] Name of the provider
  # @param content_type [String] Content type header
  # @return [BaseAdapter] Appropriate adapter for the content type
  # @raise [ExchangeRateErrors::UnsupportedFormatError] If no adapter supports the content type
  def self.for_content_type(provider_name, content_type)
    ensure_provider_registered(provider_name)
    Adapters::Services::AdapterCreator.for_content_type(provider_name, content_type)
  end

  # Create an adapter by inspecting the content
  # @param provider_name [String] Name of the provider
  # @param content [String] Content to inspect
  # @param file_extension [String] Optional file extension hint
  # @return [BaseAdapter] Appropriate adapter for the content
  # @raise [ExchangeRateErrors::UnsupportedFormatError] If no adapter supports the content
  def self.for_content(provider_name, content, file_extension = nil)
    ensure_provider_registered(provider_name)
    Adapters::Services::AdapterCreator.for_content(provider_name, content, file_extension)
  end

  # Get all available standard adapters
  # @return [Array<Class>] List of adapter classes
  def self.available_adapters
    ensure_registry_initialized
    Adapters::AdapterRegistry.instance.standard_adapters
  end

  # Register a new provider support
  # @param provider_name [String] Provider name to support
  def self.register_provider(provider_name)
    ensure_registry_initialized
    Adapters::AdapterRegistry.instance.register_provider(provider_name)
    @additional_providers << provider_name unless supported_providers.include?(provider_name)
  end

  # Register a new adapter for a provider and format
  # @param provider_name [String] Provider name
  # @param format [String] Format name (e.g., 'json', 'xml')
  # @param adapter_class [Class] Adapter class to use
  def self.register_provider_adapter(provider_name, format, adapter_class)
    ensure_registry_initialized
    register_provider(provider_name)
    Adapters::AdapterRegistry.instance.register_provider_adapter(provider_name, format, adapter_class)
  end

  private

  # Ensure the registry is initialized
  def self.ensure_registry_initialized
    initialize_registry unless Adapters::AdapterRegistry.instance.supported_providers.any?
  end

  # Ensure a provider is registered
  def self.ensure_provider_registered(provider_name)
    registry = Adapters::AdapterRegistry.instance
    unless registry.provider_supported?(provider_name)
      initialize_registry
    end
  end
end

# Initialize the registry when this file is loaded
AdapterFactory.initialize_registry