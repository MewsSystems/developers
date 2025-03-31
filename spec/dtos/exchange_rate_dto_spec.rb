require 'rails_helper'
require_relative '../../app/dtos/exchange_rate_dto'

RSpec.describe DTOs::ExchangeRateDTO do
  let(:czk) { Currency.new('CZK', 'Czech Koruna') }
  let(:usd) { Currency.new('USD', 'US Dollar') }
  let(:eur) { Currency.new('EUR', 'Euro') }
  
  let(:usd_rate) { ExchangeRate.new(from: czk, to: usd, rate: 23.117) }
  let(:eur_rate) { ExchangeRate.new(from: czk, to: eur, rate: 24.930) }
  let(:rates) { [usd_rate, eur_rate] }
  
  describe '.from_domain' do
    it 'transforms a domain ExchangeRate to a serializable hash' do
      result = described_class.from_domain(usd_rate).to_h
      
      expect(result).to be_a(Hash)
      expect(result[:from]).to eq('CZK')
      expect(result[:to]).to eq('USD')
      expect(result[:rate]).to eq(23.117)
    end
    
    it 'includes only the currency codes, not the full objects' do
      result = described_class.from_domain(eur_rate).to_h
      
      expect(result.keys).to match_array([:from, :to, :rate, :date])
      expect(result[:from]).to be_a(String)
      expect(result[:to]).to be_a(String)
    end
  end
  
  describe '.from_domain_collection' do
    it 'transforms a collection of domain ExchangeRates to an array of DTOs' do
      result = described_class.from_domain_collection(rates)
      
      expect(result).to be_an(Array)
      expect(result.size).to eq(2)
      expect(result[0]).to be_a(described_class)
      expect(result[1]).to be_a(described_class)
    end
    
    it 'returns an empty array for an empty collection' do
      result = described_class.from_domain_collection([])
      
      expect(result).to eq([])
    end
    
    it 'transforms each exchange rate correctly' do
      result = described_class.from_domain_collection(rates).map(&:to_h)
      
      expect(result[0][:to]).to eq('USD')
      expect(result[1][:to]).to eq('EUR')
    end
  end
end 