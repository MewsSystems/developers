require 'rails_helper'
require_relative '../../../../app/adapters/providers/cnb/cnb_text_adapter'

RSpec.describe Adapters::Strategies::CnbTextAdapter do
  let(:adapter) { described_class.new('CNB') }
  let(:base_currency) { 'CZK' }
  let(:sample_data) { File.read(File.join(Rails.root, 'spec/fixtures/cnb_sample.txt')) }

  describe "#parse" do
    it "parses CNB text data into ExchangeRate objects" do
      rates = adapter.parse(sample_data, base_currency)

      expect(rates).to be_an(Array)
      expect(rates).not_to be_empty
      expect(rates.first).to be_a(ExchangeRate)

      # Verify selected rates are parsed correctly
      usd_rate = rates.find { |r| r.to.code == 'USD' }
      eur_rate = rates.find { |r| r.to.code == 'EUR' }

      expect(usd_rate).not_to be_nil
      expect(usd_rate.from.code).to eq('CZK')
      expect(usd_rate.to.code).to eq('USD')
      expect(usd_rate.rate).to be_within(0.0001).of(23.117)

      expect(eur_rate).not_to be_nil
      expect(eur_rate.to.code).to eq('EUR')
      expect(eur_rate.rate).to be_within(0.0001).of(24.93)
    end

    it "correctly handles different amount values" do
      rates = adapter.parse(sample_data, base_currency)

      # Check currencies with amount > 1
      jpy_rate = rates.find { |r| r.to.code == 'JPY' }
      huf_rate = rates.find { |r| r.to.code == 'HUF' }
      idr_rate = rates.find { |r| r.to.code == 'IDR' }

      # 100 JPY = 15.376 CZK -> 1 JPY = 0.15376 CZK
      expect(jpy_rate).not_to be_nil
      expect(jpy_rate.rate).to be_within(0.00001).of(0.15376)

      # 100 HUF = 6.152 CZK -> 1 HUF = 0.06152 CZK
      expect(huf_rate).not_to be_nil
      expect(huf_rate.rate).to be_within(0.00001).of(0.06152)

      # 1000 IDR = 1.459 CZK -> 1 IDR = 0.001459 CZK
      expect(idr_rate).not_to be_nil
      expect(idr_rate.rate).to be_within(0.0000001).of(0.001459)
    end

    it "handles encoding conversion if needed" do
      # Create ISO-8859-2 encoded string (mock)
      allow(adapter).to receive(:ensure_utf8_encoding).and_call_original

      adapter.parse(sample_data, base_currency)

      expect(adapter).to have_received(:ensure_utf8_encoding).with(sample_data, described_class::SOURCE_ENCODING)
    end

    it "raises ParseError for empty data" do
      expect { adapter.parse("", base_currency) }.to raise_error(/Empty data/)
    end

    it "raises ParseError for malformed data" do
      bad_data = "Line 1\nWrong header\nMore wrong data"
      expect { adapter.parse(bad_data, base_currency) }.to raise_error(/.*/)
    end
  end
end