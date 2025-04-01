require 'rails_helper'
require_relative '../../app/adapters/adapter_factory'
require_relative '../../app/adapters/formats/json_adapter'
require_relative '../../app/adapters/formats/xml_adapter'
require_relative '../../app/adapters/formats/txt_adapter'
require_relative '../../app/adapters/providers/cnb/cnb_text_adapter'
require_relative '../../app/adapters/providers/cnb/cnb_xml_adapter'
require_relative '../../app/adapters/providers/cnb/cnb_json_adapter'
require_relative '../../app/adapters/services/adapter_creator'
require_relative '../../app/adapters/registry/adapter_registry'

RSpec.describe AdapterFactory do
  # Reset registry singleton before each test
  before do
    Adapters::AdapterRegistry.reset
    described_class.initialize_registry
    # Reset the additional providers array
    described_class.instance_variable_set(:@additional_providers, [])
  end

  describe ".for_content_type" do
    it "returns a CnbTextAdapter for text/plain content type" do
      adapter = described_class.for_content_type('CNB', 'text/plain')
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end

    it "returns a CnbXmlAdapter for application/xml content type" do
      adapter = described_class.for_content_type('CNB', 'application/xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end

    it "returns a CnbJsonAdapter for application/json content type" do
      adapter = described_class.for_content_type('CNB', 'application/json')
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)
    end

    it "handles alternative content types for the same format" do
      adapter = described_class.for_content_type('CNB', 'text/csv')
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)

      adapter = described_class.for_content_type('CNB', 'text/xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)

      adapter = described_class.for_content_type('CNB', 'text/json')
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)
    end

    it "is case-insensitive for content types" do
      adapter = described_class.for_content_type('CNB', 'TEXT/PLAIN')
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end

    it "defaults to text adapter if content_type is nil" do
      adapter = described_class.for_content_type('CNB', nil)
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end

    it "raises UnsupportedFormatError for unsupported content types" do
      expect {
        described_class.for_content_type('CNB', 'application/pdf')
      }.to raise_error(BaseAdapter::UnsupportedFormatError, /Content type.*not supported/)
    end

    it "raises UnsupportedFormatError for unsupported providers" do
      expect {
        described_class.for_content_type('UNSUPPORTED', 'text/plain')
      }.to raise_error(BaseAdapter::UnsupportedFormatError, /No adapter available/)
    end

    it 'returns TxtAdapter for text/plain' do
      adapter = described_class.for_content_type('Test', 'text/plain')
      expect(adapter).to be_a(TxtAdapter)
    end

    it 'returns XmlAdapter for application/xml' do
      adapter = described_class.for_content_type('Test', 'application/xml')
      expect(adapter).to be_a(XmlAdapter)
    end

    it 'returns JsonAdapter for application/json' do
      adapter = described_class.for_content_type('Test', 'application/json')
      expect(adapter).to be_a(JsonAdapter)
    end
  end

  describe ".for_content" do
    it "detects XML content and returns appropriate adapter" do
      xml_data = "<?xml version='1.0'?><root></root>"
      adapter = described_class.for_content('CNB', xml_data)
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)

      xml_data = "<kurzy><tabulka></tabulka></kurzy>"
      adapter = described_class.for_content('CNB', xml_data)
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end

    it "detects JSON content and returns appropriate adapter" do
      json_data = '{"key": "value"}'
      adapter = described_class.for_content('CNB', json_data)
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)

      json_data = '[1, 2, 3]'
      adapter = described_class.for_content('CNB', json_data)
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)
    end

    it "defaults to text adapter for other content" do
      data = "Some text data\nwith multiple lines"
      adapter = described_class.for_content('CNB', data)
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end

    it "uses file extension hint when provided" do
      data = "Some ambiguous data"

      adapter = described_class.for_content('CNB', data, '.xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)

      adapter = described_class.for_content('CNB', data, '.json')
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)

      adapter = described_class.for_content('CNB', data, '.txt')
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end

    it "handles file extensions with or without leading dot" do
      data = "Some ambiguous data"

      adapter = described_class.for_content('CNB', data, 'xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)

      adapter = described_class.for_content('CNB', data, '.json')
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)
    end
  end

  describe 'delegation' do
    describe '.for_file_extension' do
      it 'delegates to AdapterCreator.for_file_extension' do
        expect(Adapters::Services::AdapterCreator).to receive(:for_file_extension).with('CNB', 'xml').and_call_original
        described_class.for_file_extension('CNB', 'xml')
      end

      it 'initializes registry before adapter creation' do
        # Create a registry that doesn't know about our provider
        Adapters::AdapterRegistry.reset
        registry = Adapters::AdapterRegistry.instance

        # Check that our provider is not supported
        expect(registry.provider_supported?('TestNewProvider')).to be false

        # Spy on initialize_registry
        allow(described_class).to receive(:initialize_registry).and_call_original

        # This will fail but we don't care
        begin
          described_class.for_file_extension('TestNewProvider', 'xml')
        rescue BaseAdapter::UnsupportedFormatError
          # Expected
        end

        # Verify initialize_registry was called
        expect(described_class).to have_received(:initialize_registry)
      end
    end

    describe '.for_content_type' do
      it 'delegates to AdapterCreator.for_content_type' do
        expect(Adapters::Services::AdapterCreator).to receive(:for_content_type).with('CNB',
                                                                                      'application/xml').and_call_original
        described_class.for_content_type('CNB', 'application/xml')
      end
    end

    describe '.for_content' do
      it 'delegates to AdapterCreator.for_content' do
        expect(Adapters::Services::AdapterCreator).to receive(:for_content).with('CNB', 'content',
                                                                                 'xml').and_call_original
        described_class.for_content('CNB', 'content', 'xml')
      end
    end

    describe '.available_adapters' do
      it 'returns adapters from the registry' do
        adapters = [JsonAdapter, XmlAdapter, TxtAdapter]
        expect(Adapters::AdapterRegistry.instance).to receive(:standard_adapters).and_return(adapters)
        expect(described_class.available_adapters).to eq(adapters)
      end
    end

    describe '.register_provider' do
      it 'registers a provider with the registry' do
        expect {
          described_class.register_provider('NewTestProvider')
        }.to change {
          Adapters::AdapterRegistry.instance.supported_providers.include?('NewTestProvider')
        }.from(false).to(true)
      end

      it 'adds the provider to additional_providers' do
        expect {
          described_class.register_provider('AnotherProvider')
        }.to change {
          described_class.additional_providers.include?('AnotherProvider')
        }.from(false).to(true)
      end
    end

    describe '.register_provider_adapter' do
      it 'registers a provider adapter with the registry' do
        provider = 'TestCustomProvider'
        format = 'custom'
        adapter_class = JsonAdapter

        described_class.register_provider_adapter(provider, format, adapter_class)

        registry = Adapters::AdapterRegistry.instance
        expect(registry.provider_adapter(provider, format)).to eq(adapter_class)
      end
    end

    describe '.supported_providers' do
      it 'returns both default and additional providers' do
        # Add a new provider
        described_class.register_provider('NewDynamicProvider')

        # Verify it's in the supported providers list
        expect(described_class.supported_providers).to include('CNB', 'Test', 'ECB', 'NewDynamicProvider')
      end
    end
  end
end