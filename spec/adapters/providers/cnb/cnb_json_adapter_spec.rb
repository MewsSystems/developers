require 'rails_helper'
require_relative '../../../../app/adapters/providers/cnb/cnb_json_adapter'

RSpec.describe Adapters::Strategies::CnbJsonAdapter do
  describe "#parse" do
    let(:adapter) { described_class.new }
    let(:base_currency) { 'CZK' }
    let(:sample_data) do
      {
        rates: [
          { code: 'USD', rate: 21.123, amount: 1 },
          { code: 'EUR', rate: 23.456, amount: 1 }
        ],
        date: '2023-10-15'
      }.to_json
    end
    
    it "parses the CNB JSON data into ExchangeRate objects" do
      rates = adapter.parse(sample_data, base_currency)
      
      expect(rates).to be_an(Array)
      expect(rates).not_to be_empty
      
      # Check specific rates
      usd_rate = rates.find { |r| r.to.code == 'USD' }
      expect(usd_rate).not_to be_nil
      expect(usd_rate.from.code).to eq(base_currency)
      expect(usd_rate.rate).to eq(21.123)
      
      eur_rate = rates.find { |r| r.to.code == 'EUR' }
      expect(eur_rate).not_to be_nil
      expect(eur_rate.rate).to eq(23.456)
    end
    
    it "raises ParseError for invalid JSON" do
      bad_data = '{invalid-json'
      expect { adapter.parse(bad_data, base_currency) }.to raise_error(ExchangeRateErrors::ParseError) do |error|
        expect(error.message).to include("Error parsing CNB JSON feed")
      end
    end
    
    it "raises ParseError for JSON missing expected structure" do
      bad_data = '{"invalid": true}'
      expect { adapter.parse(bad_data, base_currency) }.to raise_error(ExchangeRateErrors::ParseError) do |error|
        expect(error.message).to include("No valid exchange rates found")
      end
    end
    
    it "raises ParseError for empty array of rates" do
      bad_data = '{"rates": []}'
      expect { adapter.parse(bad_data, base_currency) }.to raise_error(ExchangeRateErrors::ParseError) do |error|
        expect(error.message).to include("No valid exchange rates found")
      end
    end
  end
end 