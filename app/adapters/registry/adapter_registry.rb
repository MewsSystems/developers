require_relative '../../errors/exchange_rate_errors'

module Adapters
  class AdapterRegistry
    # Singleton instance
    @instance = nil

    # Get the singleton instance
    def self.instance
      @instance ||= new
    end

    # Reset registry (mainly for testing)
    def self.reset
      @instance = new
    end

    def initialize
      # Standard adapters by priority
      @standard_adapters = []

      # Provider-specific adapters
      @provider_adapters = {}

      # Extension to adapter mappings
      @extension_adapters = {}

      # Supported providers
      @supported_providers = []
    end

    # Register a standard adapter
    # @param adapter_class [Class] Adapter class to register
    def register_standard_adapter(adapter_class)
      @standard_adapters << adapter_class unless @standard_adapters.include?(adapter_class)
    end

    # Register a provider-specific adapter
    # @param provider [String] Provider name
    # @param format [String] Format name
    # @param adapter_class [Class] Adapter class to register
    def register_provider_adapter(provider, format, adapter_class)
      @provider_adapters[provider] ||= {}
      @provider_adapters[provider][format] = adapter_class
      register_provider(provider)
    end

    # Register a file extension adapter
    # @param extension [String] File extension
    # @param adapter_class [Class] Adapter class to register
    def register_extension_adapter(extension, adapter_class)
      @extension_adapters[extension.to_s.downcase.delete('.')] = adapter_class
    end

    # Register a provider
    # @param provider [String] Provider name
    def register_provider(provider)
      @supported_providers << provider unless @supported_providers.include?(provider)
    end

    # Get all standard adapters
    # @return [Array<Class>] List of adapter classes
    def standard_adapters
      @standard_adapters.dup
    end

    # Get provider-specific adapter for a format
    # @param provider [String] Provider name
    # @param format [String] Format name
    # @return [Class, nil] Adapter class or nil if not found
    def provider_adapter(provider, format)
      return nil unless @provider_adapters[provider]

      @provider_adapters[provider][format]
    end

    # Get adapter for a file extension
    # @param extension [String] File extension
    # @return [Class, nil] Adapter class or nil if not found
    def extension_adapter(extension)
      @extension_adapters[extension.to_s.downcase.delete('.')]
    end

    # Get supported formats for a provider
    # @param provider [String] Provider name
    # @return [Array<String>] List of supported formats
    def provider_formats(provider)
      return [] unless @provider_adapters[provider]

      @provider_adapters[provider].keys
    end

    # Get all supported providers
    # @return [Array<String>] List of supported providers
    def supported_providers
      @supported_providers.dup
    end

    # Check if a provider is supported
    # @param provider_name [String] Provider name
    # @return [Boolean] Whether the provider is supported
    def provider_supported?(provider_name)
      @supported_providers.include?(provider_name)
    end

    # Check if a provider is supported
    # @param provider_name [String] Provider name
    # @raise [BaseAdapter::UnsupportedFormatError] If the provider is not supported
    def validate_provider_support(provider_name)
      return if provider_supported?(provider_name)

      raise BaseAdapter::UnsupportedFormatError, "No adapter available for provider #{provider_name}"
    end
  end
end