require 'rails_helper'
require_relative '../../../app/adapters/services/adapter_creator'
require_relative '../../../app/adapters/formats/json_adapter'
require_relative '../../../app/adapters/formats/xml_adapter'
require_relative '../../../app/adapters/formats/txt_adapter'
require_relative '../../../app/adapters/providers/cnb/cnb_text_adapter'
require_relative '../../../app/adapters/providers/cnb/cnb_xml_adapter'
require_relative '../../../app/adapters/providers/cnb/cnb_json_adapter'

RSpec.describe Adapters::Services::AdapterCreator do
  let(:registry) { Adapters::AdapterRegistry.instance }
  
  before(:each) do
    # Reset registry and register standard adapters
    Adapters::AdapterRegistry.reset
    registry.register_standard_adapter(JsonAdapter)
    registry.register_standard_adapter(XmlAdapter)
    registry.register_standard_adapter(TxtAdapter)
    
    # Register CNB adapters
    registry.register_provider('CNB')
    registry.register_provider_adapter('CNB', 'xml', Adapters::Strategies::CnbXmlAdapter)
    registry.register_provider_adapter('CNB', 'json', Adapters::Strategies::CnbJsonAdapter)
    registry.register_provider_adapter('CNB', 'txt', Adapters::Strategies::CnbTextAdapter)
    
    # Register test provider
    registry.register_provider('Test')
    
    # Register file extensions
    registry.register_extension_adapter('json', JsonAdapter)
    registry.register_extension_adapter('xml', XmlAdapter)
    registry.register_extension_adapter('txt', TxtAdapter)
  end
  
  describe '.for_file_extension' do
    it 'returns a provider-specific adapter when available' do
      adapter = described_class.for_file_extension('CNB', 'xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end
    
    it 'returns a general adapter when no provider-specific adapter exists' do
      adapter = described_class.for_file_extension('Test', 'json')
      expect(adapter).to be_a(JsonAdapter)
    end
    
    it 'normalizes file extensions' do
      adapter = described_class.for_file_extension('CNB', '.XmL')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end
    
    it 'raises UnsupportedFormatError for unknown file extensions' do
      expect {
        described_class.for_file_extension('CNB', 'pdf')
      }.to raise_error(/No adapter found for file extension/)
    end
    
    it 'raises UnsupportedFormatError for unsupported providers' do
      expect {
        described_class.for_file_extension('Unknown', 'xml')
      }.to raise_error(/No adapter available for provider/)
    end
  end
  
  describe '.for_content_type' do
    it 'returns a provider-specific adapter when available' do
      adapter = described_class.for_content_type('CNB', 'application/xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end
    
    it 'returns a general adapter when no provider-specific adapter exists' do
      adapter = described_class.for_content_type('Test', 'application/json')
      expect(adapter).to be_a(JsonAdapter)
    end
    
    it 'defaults to text adapter when content_type is nil' do
      adapter = described_class.for_content_type('CNB', nil)
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end
    
    it 'raises UnsupportedFormatError for unknown content types' do
      expect {
        described_class.for_content_type('Test', 'application/pdf')
      }.to raise_error(/No adapter found for content type/)
    end
    
    it 'raises a specialized error for CNB provider with unknown content types' do
      expect {
        described_class.for_content_type('CNB', 'application/pdf')
      }.to raise_error(BaseAdapter::UnsupportedFormatError, /Content type.*not supported/)
    end
  end
  
  describe '.for_content' do
    it 'detects XML content and returns appropriate provider-specific adapter' do
      xml_data = '<?xml version="1.0"?><root></root>'
      adapter = described_class.for_content('CNB', xml_data)
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end
    
    it 'detects JSON content and returns appropriate provider-specific adapter' do
      json_data = '{"key": "value"}'
      adapter = described_class.for_content('CNB', json_data)
      expect(adapter).to be_a(Adapters::Strategies::CnbJsonAdapter)
    end
    
    it 'defaults to text adapter for content that is not XML or JSON' do
      data = 'plain text data'
      adapter = described_class.for_content('CNB', data)
      expect(adapter).to be_a(Adapters::Strategies::CnbTextAdapter)
    end
    
    it 'uses file extension hint when provided' do
      data = 'This could be anything'
      adapter = described_class.for_content('CNB', data, 'xml')
      expect(adapter).to be_a(Adapters::Strategies::CnbXmlAdapter)
    end
    
    it 'returns a general adapter for non-specific providers' do
      xml_data = '<?xml version="1.0"?><root></root>'
      adapter = described_class.for_content('Test', xml_data)
      expect(adapter).to be_a(XmlAdapter)
    end
  end
end 