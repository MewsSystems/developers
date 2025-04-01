require 'rails_helper'
require_relative '../../app/adapters/adapter_factory'
require_relative '../../app/adapters/formats/json_adapter'
require_relative '../../app/adapters/formats/xml_adapter'
require_relative '../../app/adapters/formats/txt_adapter'
require_relative '../../app/adapters/providers/cnb/cnb_text_adapter'
require_relative '../../app/adapters/providers/cnb/cnb_xml_adapter'
require_relative '../../app/adapters/providers/cnb/cnb_json_adapter'

RSpec.describe AdapterFactory do
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

  describe '.for_content_type' do
    it 'returns TxtAdapter for text/plain' do
      adapter = AdapterFactory.for_content_type('Test', 'text/plain')
      expect(adapter).to be_a(TxtAdapter)
    end

    it 'returns XmlAdapter for application/xml' do
      adapter = AdapterFactory.for_content_type('Test', 'application/xml')
      expect(adapter).to be_a(XmlAdapter)
    end

    it 'returns JsonAdapter for application/json' do
      adapter = AdapterFactory.for_content_type('Test', 'application/json')
      expect(adapter).to be_a(JsonAdapter)
    end
  end
end