require 'rails_helper'
require_relative '../../../app/adapters/base/base_adapter'

RSpec.describe BaseAdapter do
  let(:adapter) { BaseAdapter.new('test_provider') }

  describe "#parse" do
    it "raises NotImplementedError" do
      expect { adapter.parse("data", "CZK") }.to raise_error(NotImplementedError, /must implement/)
    end
  end

  describe "#ensure_utf8_encoding" do
    it "returns the string as is if already UTF-8" do
      utf8_string = "Test string".encode('UTF-8')
      result = adapter.send(:ensure_utf8_encoding, utf8_string)
      expect(result).to eq(utf8_string)
      expect(result.encoding.name).to eq('UTF-8')
    end

    it "converts from another encoding to UTF-8" do
      # Create a string in ISO-8859-1 encoding
      iso_string = "Test string with special chars: åäö".encode('ISO-8859-1')
      expect(iso_string.encoding.name).to eq('ISO-8859-1')

      result = adapter.send(:ensure_utf8_encoding, iso_string, 'ISO-8859-1')
      expect(result.encoding.name).to eq('UTF-8')
    end

    it "raises error for invalid encoding conversion" do
      # Create a binary string that's not valid in the source encoding
      invalid_string = "Invalid \xFF encoding".force_encoding('BINARY')

      expect {
        adapter.send(:ensure_utf8_encoding, invalid_string, 'US-ASCII')
      }.to raise_error(ExchangeRateErrors::ParseError, /Encoding conversion error/)
    end
  end

  describe "#parse_rate" do
    it "parses valid rate strings" do
      expect(adapter.send(:parse_rate, "23.117", "USD")).to eq(23.117)
      # Handle comma as decimal separator
      expect(adapter.send(:parse_rate, "23,117", "USD")).to eq(23.117)
    end

    it "raises error for non-positive rates" do
      expect {
        adapter.send(:parse_rate, "0", "USD")
      }.to raise_error(/Invalid non-positive rate/)

      expect {
        adapter.send(:parse_rate, "-5.0", "USD")
      }.to raise_error(/Invalid non-positive rate/)
    end

    it "raises error for invalid rate format" do
      expect {
        adapter.send(:parse_rate, "not-a-number", "USD")
      }.to raise_error(/Invalid rate format/)
    end
  end

  describe "#parse_amount" do
    it "parses valid amount strings" do
      expect(adapter.send(:parse_amount, "1", "USD")).to eq(1)
      expect(adapter.send(:parse_amount, "100", "JPY")).to eq(100)
    end

    it "raises error for non-positive amounts" do
      expect {
        adapter.send(:parse_amount, "0", "USD")
      }.to raise_error(/Invalid non-positive amount/)

      expect {
        adapter.send(:parse_amount, "-5", "USD")
      }.to raise_error(/Invalid non-positive amount/)
    end

    it "raises error for invalid amount format" do
      expect {
        adapter.send(:parse_amount, "not-a-number", "USD")
      }.to raise_error(/Invalid amount format/)
    end
  end

  describe "#create_currency" do
    it "creates a Currency object" do
      currency = adapter.send(:create_currency, "USD", "US Dollar")
      expect(currency).to be_a(Currency)
      expect(currency.code).to eq("USD")
      expect(currency.name).to eq("US Dollar")
    end
  end

  describe "#create_exchange_rate" do
    let(:czk) { Currency.new('CZK') }
    let(:usd) { Currency.new('USD') }

    it "creates an ExchangeRate object" do
      rate = adapter.send(:create_exchange_rate, czk, usd, 23.117)
      expect(rate).to be_an(ExchangeRate)
      expect(rate.from).to eq(czk)
      expect(rate.to).to eq(usd)
      expect(rate.rate).to eq(23.117)
    end

    it "normalizes rates based on amount" do
      # 100 JPY = 15.376 CZK -> 1 JPY = 0.15376 CZK
      jpy = Currency.new('JPY')
      rate = adapter.send(:create_exchange_rate, czk, jpy, 15.376, 100)
      expect(rate.rate).to be_within(0.00001).of(0.15376)
    end
  end
end