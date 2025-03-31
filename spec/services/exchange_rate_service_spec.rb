require 'rails_helper'
require_relative '../../app/services/exchange_rate/rate_service'
require_relative '../../app/providers/exchange_rate_provider_interface'

RSpec.describe RateService do
  let(:provider) do
    # Create a mock provider that implements the ExchangeRateProviderInterface
    instance_double("BaseProvider").tap do |p|
      allow(p).to receive(:respond_to?).with(:fetch_rates).and_return(true)
      allow(p).to receive(:respond_to?).with(:metadata).and_return(true)
      allow(p).to receive(:fetch_rates).and_return([])
      allow(p).to receive(:metadata).and_return({
        update_frequency: :daily,
        publication_time: Time.new(2025, 3, 26, 14, 30, 0, "+01:00"),
        working_days_only: true,
        supports_historical: true,
        base_currency: 'CZK',
        supported_currencies: ['USD', 'EUR', 'JPY', 'GBP'],
        source_name: 'Test Provider'
      })
      allow(p).to receive(:supported_currencies).and_return(['USD', 'EUR', 'JPY', 'GBP'])
    end
  end
  
  let(:repository) { instance_double("ExchangeRateRepository") }
  let(:cache_strategy) { instance_double("CacheStrategy") }
  let(:service) { RateService.new(provider, repository, cache_strategy) }
  
  let(:today) { Date.new(2025, 3, 26) } # Wednesday
  let(:yesterday) { Date.new(2025, 3, 25) } # Tuesday
  let(:friday) { Date.new(2025, 3, 21) } # Previous Friday
  
  let(:czk) { Currency.new('CZK') }
  let(:usd) { Currency.new('USD') }
  let(:eur) { Currency.new('EUR') }
  let(:jpy) { Currency.new('JPY') }
  
  let(:rates) do
    [
      ExchangeRate.new(from: czk, to: usd, rate: 23.117),
      ExchangeRate.new(from: czk, to: eur, rate: 24.930),
      ExchangeRate.new(from: czk, to: jpy, rate: 0.15376)
    ]
  end

  before do
    allow(Date).to receive(:today).and_return(today)
    allow(cache_strategy).to receive(:determine_fetch_date).and_return(today)
  end

  describe "#get_rates" do
    context "with cached rates" do
      before do
        allow(cache_strategy).to receive(:get_cached_rates).and_return(rates)
      end

      it "returns cached rates without calling provider" do
        expect(provider).not_to receive(:fetch_rates)
        result = service.get_rates
        expect(result).to eq(rates)
      end
    end

    context "without cached rates" do
      before do
        allow(cache_strategy).to receive(:get_cached_rates).and_return(nil)
        allow(provider).to receive(:fetch_rates).and_return(rates)
        allow(repository).to receive(:save_for)
      end

      it "fetches rates from provider" do
        expect(provider).to receive(:fetch_rates).and_return(rates)
        expect(repository).to receive(:save_for).with(today, rates)
        
        result = service.get_rates
        expect(result).to eq(rates)
      end
    end

    context "when filtering by currency" do
      before do
        allow(cache_strategy).to receive(:get_cached_rates).and_return(rates)
        allow(service).to receive(:log_unavailable_currency)
      end

      it "returns filtered rates when currencies specified" do
        result = service.get_rates(['USD'])
        expect(result.size).to eq(1)
        expect(result.first.to.code).to eq('USD')
      end

      it "handles case insensitivity" do
        result = service.get_rates(['usd', 'eur'])
        expect(result.size).to eq(2)
        codes = result.map { |r| r.to.code }.sort
        expect(codes).to eq(['EUR', 'USD'])
      end
    end
  end

  describe "#initialize" do
    it "requires a provider that implements the interface" do
      invalid_provider = "not a provider"
      expect { RateService.new(invalid_provider, repository) }
        .to raise_error(ExchangeRateErrors::InvalidConfigurationError)
    end

    it "accepts a valid provider" do
      allow(provider).to receive(:fetch_rates).and_return([])
      expect { RateService.new(provider, repository) }.not_to raise_error
    end
  end

  describe "#unavailable_currencies" do
    before do
      allow(cache_strategy).to receive(:get_cached_rates).and_return(rates)
      allow(service).to receive(:log_unavailable_currency)
    end

    it "tracks unavailable currencies" do
      # Allow method to run
      allow(service).to receive(:check_currency_availability).and_call_original
      
      # Request unavailable currency
      service.get_rates(['GBP'])
      
      # Check tracking
      unavailable = service.unavailable_currencies
      expect(unavailable.keys).to include('GBP')
    end
  end
end 