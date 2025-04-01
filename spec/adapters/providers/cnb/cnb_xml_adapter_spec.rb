require 'rails_helper'
require_relative '../../../../app/adapters/providers/cnb/cnb_xml_adapter'

RSpec.describe Adapters::Strategies::CnbXmlAdapter do
  describe "#parse" do
    let(:adapter) { described_class.new }
    let(:base_currency) { 'CZK' }
    let(:sample_data) {
      <<~XML
        <?xml version="1.0" encoding="UTF-8"?>
        <kurzy banka="CNB" datum="26.03.2025" poradi="57">
          <tabulka typ="CNB_KURZY_DEVIZOVEHO_TRHU">
            <radek kod="USD" mena="dolar" mnozstvi="1" kurz="23.117" />
            <radek kod="EUR" mena="euro" mnozstvi="1" kurz="24.930" />
            <radek kod="JPY" mena="jen" mnozstvi="100" kurz="15.376" />
          </tabulka>
        </kurzy>
      XML
    }

    it "parses the CNB XML data into ExchangeRate objects" do
      rates = adapter.parse(sample_data, base_currency)

      expect(rates).to be_an(Array)
      expect(rates).not_to be_empty

      # Check a specific rate
      usd_rate = rates.find { |r| r.to.code == 'USD' }
      expect(usd_rate).not_to be_nil
      expect(usd_rate.from.code).to eq(base_currency)
      expect(usd_rate.to.code).to eq('USD')
      expect(usd_rate.rate).to eq(23.117)

      # Check a rate with amount > 1
      jpy_rate = rates.find { |r| r.to.code == 'JPY' }
      expect(jpy_rate).not_to be_nil
      expect(jpy_rate.rate).to be_within(0.00001).of(0.15376) # 100 JPY = 15.376 CZK, so 1 JPY = 0.15376 CZK
    end

    it "handles encoding conversion if needed" do
      # Since we've moved the encoding conversion to the utility class
      # we just need to make sure parsing works
      rates = adapter.parse(sample_data, base_currency)
      expect(rates).to be_an(Array)
      expect(rates).not_to be_empty
    end

    it "raises ParseError for invalid XML" do
      bad_data = '<invalid>xml data'
      expect { adapter.parse(bad_data, base_currency) }.to raise_error(ExchangeRateErrors::ParseError) do |error|
        expect(error.message).to include("Error parsing CNB XML feed")
      end
    end

    it "raises ParseError for XML missing expected elements" do
      bad_data = '<empty></empty>'
      expect { adapter.parse(bad_data, base_currency) }.to raise_error(ExchangeRateErrors::ParseError) do |error|
        expect(error.message).to include("No valid exchange rates found")
      end
    end
  end

  describe "#supports_content_type?" do
    let(:adapter) { described_class.new }

    it "returns true for XML content types" do
      expect(adapter.supports_content_type?("application/xml")).to be true
      expect(adapter.supports_content_type?("text/xml")).to be true
    end

    it "returns false for non-XML content types" do
      expect(adapter.supports_content_type?("application/json")).to be false
      expect(adapter.supports_content_type?("text/plain")).to be false
    end

    it "returns false for nil content type" do
      expect(adapter.supports_content_type?(nil)).to be false
    end
  end

  describe "#supports_content?" do
    let(:adapter) { described_class.new }

    it "returns true for XML content" do
      expect(adapter.supports_content?("<?xml version=\"1.0\"?><root></root>")).to be true
      expect(adapter.supports_content?("<kurzy></kurzy>")).to be true
    end

    it "returns false for non-XML content" do
      expect(adapter.supports_content?("{\"key\": \"value\"}")).to be false
      expect(adapter.supports_content?("plain text")).to be false
    end

    it "returns false for nil content" do
      expect(adapter.supports_content?(nil)).to be false
    end
  end
end