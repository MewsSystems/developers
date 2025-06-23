require_relative 'adapter_registry'
require_relative '../formats/txt_adapter'
require_relative '../formats/xml_adapter'
require_relative '../formats/json_adapter'
require_relative '../providers/cnb/cnb_text_adapter'
require_relative '../providers/cnb/cnb_xml_adapter'
require_relative '../providers/cnb/cnb_json_adapter'

module Adapters
  module Registry
    # Responsible for initializing the adapter registry with default adapters
    class RegistryInitializer
      # Initialize the adapter registry with default adapters
      # @param providers [Array<String>] List of providers to register
      def self.initialize_registry(providers)
        registry = Adapters::AdapterRegistry.instance

        register_standard_adapters(registry)
        register_provider_adapters(registry)
        register_extension_mappings(registry)
        register_providers(registry, providers)
      end

      # Register standard adapters in order of preference
      # @param registry [AdapterRegistry] Registry instance
      def self.register_standard_adapters(registry)
        registry.register_standard_adapter(JsonAdapter)
        registry.register_standard_adapter(XmlAdapter)
        registry.register_standard_adapter(TxtAdapter)
      end

      # Register provider-specific adapters
      # @param registry [AdapterRegistry] Registry instance
      def self.register_provider_adapters(registry)
        # Register CNB specific adapters
        registry.register_provider_adapter('CNB', 'xml', Adapters::Strategies::CnbXmlAdapter)
        registry.register_provider_adapter('CNB', 'json', Adapters::Strategies::CnbJsonAdapter)
        registry.register_provider_adapter('CNB', 'txt', Adapters::Strategies::CnbTextAdapter)
        registry.register_provider_adapter('CNB', 'text', Adapters::Strategies::CnbTextAdapter)
        registry.register_provider_adapter('CNB', 'csv', Adapters::Strategies::CnbTextAdapter)
      end

      # Register file extension to adapter mappings
      # @param registry [AdapterRegistry] Registry instance
      def self.register_extension_mappings(registry)
        registry.register_extension_adapter('json', JsonAdapter)
        registry.register_extension_adapter('xml', XmlAdapter)
        registry.register_extension_adapter('txt', TxtAdapter)
        registry.register_extension_adapter('text', TxtAdapter)
        registry.register_extension_adapter('csv', TxtAdapter)
      end

      # Register supported providers
      # @param registry [AdapterRegistry] Registry instance
      # @param providers [Array<String>] List of providers to register
      def self.register_providers(registry, providers)
        providers.each { |provider| registry.register_provider(provider) }
      end
    end
  end
end