require 'rails_helper'
require_relative '../../../app/adapters/registry/adapter_registry'
require_relative '../../../app/adapters/formats/json_adapter'
require_relative '../../../app/adapters/formats/xml_adapter'
require_relative '../../../app/adapters/formats/txt_adapter'

RSpec.describe Adapters::AdapterRegistry do
  let(:registry) { described_class.new }

  # Reset the singleton instance after each test
  after do
    described_class.reset
  end

  describe '#instance' do
    it 'returns a singleton instance' do
      instance1 = described_class.instance
      instance2 = described_class.instance
      expect(instance1).to be(instance2)
    end

    it 'can be reset' do
      instance1 = described_class.instance
      described_class.reset
      instance2 = described_class.instance
      expect(instance1).not_to be(instance2)
    end
  end

  describe '#register_standard_adapter' do
    it 'registers a standard adapter' do
      registry.register_standard_adapter(JsonAdapter)
      expect(registry.standard_adapters).to include(JsonAdapter)
    end

    it 'does not register the same adapter twice' do
      registry.register_standard_adapter(JsonAdapter)
      registry.register_standard_adapter(JsonAdapter)
      expect(registry.standard_adapters.count(JsonAdapter)).to eq(1)
    end
  end

  describe '#register_provider_adapter' do
    it 'registers a provider-specific adapter' do
      registry.register_provider_adapter('Test', 'json', JsonAdapter)
      adapter_class = registry.provider_adapter('Test', 'json')
      expect(adapter_class).to eq(JsonAdapter)
    end

    it 'automatically registers the provider' do
      registry.register_provider_adapter('Test', 'json', JsonAdapter)
      expect(registry.supported_providers).to include('Test')
    end
  end

  describe '#register_extension_adapter' do
    it 'registers an extension adapter' do
      registry.register_extension_adapter('json', JsonAdapter)
      adapter_class = registry.extension_adapter('json')
      expect(adapter_class).to eq(JsonAdapter)
    end

    it 'normalizes the extension' do
      registry.register_extension_adapter('.JSON', JsonAdapter)
      adapter_class = registry.extension_adapter('json')
      expect(adapter_class).to eq(JsonAdapter)
    end
  end

  describe '#register_provider' do
    it 'registers a provider' do
      registry.register_provider('Test')
      expect(registry.supported_providers).to include('Test')
    end

    it 'does not register the same provider twice' do
      registry.register_provider('Test')
      registry.register_provider('Test')
      expect(registry.supported_providers.count('Test')).to eq(1)
    end
  end

  describe '#provider_adapter' do
    it 'returns the registered adapter for a provider and format' do
      registry.register_provider_adapter('Test', 'json', JsonAdapter)
      adapter_class = registry.provider_adapter('Test', 'json')
      expect(adapter_class).to eq(JsonAdapter)
    end

    it 'returns nil if no adapter is registered for the provider' do
      adapter_class = registry.provider_adapter('Unknown', 'json')
      expect(adapter_class).to be_nil
    end

    it 'returns nil if no adapter is registered for the format' do
      registry.register_provider('Test')
      adapter_class = registry.provider_adapter('Test', 'unknown')
      expect(adapter_class).to be_nil
    end
  end

  describe '#provider_formats' do
    it 'returns registered formats for a provider' do
      registry.register_provider_adapter('Test', 'json', JsonAdapter)
      registry.register_provider_adapter('Test', 'xml', XmlAdapter)
      formats = registry.provider_formats('Test')
      expect(formats).to contain_exactly('json', 'xml')
    end

    it 'returns an empty array for unknown providers' do
      formats = registry.provider_formats('Unknown')
      expect(formats).to be_empty
    end
  end

  describe '#validate_provider_support' do
    it 'does not raise an error for supported providers' do
      registry.register_provider('Test')
      expect { registry.validate_provider_support('Test') }.not_to raise_error
    end

    it 'raises UnsupportedFormatError for unsupported providers' do
      expect { registry.validate_provider_support('Unknown') }
        .to raise_error(BaseAdapter::UnsupportedFormatError, /No adapter available/)
    end
  end
end