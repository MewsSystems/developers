require 'rails_helper'

RSpec.describe 'Exchange Rates API', type: :request do
  # Setup our routes directly for integration tests
  before(:all) do
    Rails.application.routes.draw do
      # API routes - versioned and namespaced
      namespace :api do
        namespace :v1 do
          get '/exchange_rates', to: 'exchange_rates#index'
          get '/exchange_rates/convert', to: 'exchange_rates#convert'
          get '/exchange_rates/currencies', to: 'exchange_rates#currencies'
          get '/exchange_rates/:from/:to', to: 'exchange_rates#show'
        end
      end

      # Legacy routes - map to API endpoints for backward compatibility
      get '/exchange_rates', to: 'api/v1/exchange_rates#index'
      get '/exchange_rates/convert', to: 'api/v1/exchange_rates#convert'
      get '/exchange_rates/supported_currencies', to: 'api/v1/exchange_rates#currencies'

      # Health check for Docker and monitoring
      get '/health', to: 'health#index'

      # Debug routes for development only
      namespace :debug do
        get '/redis', to: 'redis#index'
      end
    end
  end

  # Reload routes after all tests in this file
  after(:all) do
    Rails.application.reload_routes!
  end

  # Create test class to represent the exchange rate service
  class MockExchangeRateService
    attr_reader :unavailable_currencies, :provider

    def initialize
      @unavailable_currencies = {}
      @provider = MockCNBProvider.new
    end

    def get_rates(currencies = nil)
      sample_rates = [
        ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('USD'), rate: 23.117),
        ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('EUR'), rate: 24.930),
        ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('JPY'), rate: 0.15376),
        ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('GBP'), rate: 29.178)
      ]

      if currencies.nil?
        sample_rates
      elsif currencies.include?('INVALID')
        @unavailable_currencies['INVALID'] = Time.now
        sample_rates.select { |rate| currencies.include?(rate.to.code) }
      else
        sample_rates.select { |rate| currencies.include?(rate.to.code) }
      end
    end

    def convert(amount, from, to)
      {
        from: from,
        to: to,
        amount: amount,
        converted_amount: amount * 0.95,
        rate: 0.95
      }
    end
  end

  # Create test class to represent the provider
  class MockCNBProvider
    def metadata
      {
        source_name: 'Czech National Bank (CNB)',
        base_currency: 'CZK',
        update_frequency: :daily,
        supported_currencies: ['USD', 'EUR', 'JPY', 'GBP']
      }
    end

    def supported_currencies
      ['USD', 'EUR', 'JPY', 'GBP']
    end
  end

  before do
    # Mock the controller methods to return our test instances
    mock_service = MockExchangeRateService.new

    # For the API controller
    allow_any_instance_of(Api::V1::ExchangeRatesController).to receive(:exchange_rate_service).and_return(mock_service)
  end

  describe 'Legacy API Endpoints' do
    it 'returns all available exchange rates' do
      get '/exchange_rates'

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      expect(json_response['rates'].size).to eq(4)

      usd_rate = json_response['rates'].find { |r| r['to'] == 'USD' }
      expect(usd_rate).to include('from', 'to', 'rate')
      expect(usd_rate['from']).to eq('CZK')
      expect(usd_rate['to']).to eq('USD')
      expect(usd_rate['rate']).to eq(23.117)
    end

    it 'filters rates by the currencies parameter' do
      get '/exchange_rates', params: { currencies: 'USD,EUR' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      expect(json_response['rates'].size).to eq(2)
      currencies = json_response['rates'].map { |r| r['to'] }
      expect(currencies).to match_array(['USD', 'EUR'])
    end

    it 'returns a single currency when only one is requested' do
      get '/exchange_rates', params: { currencies: 'JPY' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      expect(json_response['rates'].size).to eq(1)
      expect(json_response['rates'][0]['to']).to eq('JPY')
      expect(json_response['rates'][0]['rate']).to be_within(0.00001).of(0.15376)
    end

    it 'returns empty rates array when requested currencies are not found' do
      get '/exchange_rates', params: { currencies: 'INVALID' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      expect(json_response['rates']).to be_empty

      # Should include warnings about unavailable currencies
      expect(json_response).to have_key('warnings')
      expect(json_response['warnings']['unavailable_currencies']).to include('INVALID')
    end

    it 'includes both available and unavailable currencies in mixed requests' do
      get '/exchange_rates', params: { currencies: 'USD,INVALID' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      # Should have the available currency rate
      expect(json_response['rates'].size).to eq(1)
      expect(json_response['rates'][0]['to']).to eq('USD')

      # Should include warning about unavailable currency
      expect(json_response).to have_key('warnings')
      expect(json_response['warnings']['unavailable_currencies']).to include('INVALID')
      expect(json_response['warnings']).to have_key('available_currencies')
    end

    it 'includes provider name in warnings section' do
      get '/exchange_rates', params: { currencies: 'INVALID' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      expect(json_response).to have_key('warnings')
      expect(json_response['warnings']).to have_key('provider')
      expect(json_response['warnings']['provider']).to include('CNB')
    end

    it 'handles network errors gracefully' do
      # Create an error-throwing service
      error_service = MockExchangeRateService.new
      allow(error_service).to receive(:get_rates).and_raise(StandardError.new("Network error"))

      # Replace our service with the error one just for this test
      allow_any_instance_of(Api::V1::ExchangeRatesController)
        .to receive(:exchange_rate_service)
        .and_return(error_service)

      get '/exchange_rates'

      expect(response).to have_http_status(:internal_server_error)
      json_response = JSON.parse(response.body)
      expect(json_response).to have_key('error')
    end

    it 'converts currency amounts correctly' do
      get '/exchange_rates/convert', params: { from: 'USD', to: 'EUR', amount: 100 }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      expect(json_response['from']).to eq('USD')
      expect(json_response['to']).to eq('EUR')
      expect(json_response['amount']).to eq(100)
      expect(json_response['converted_amount']).to eq(95)
    end
  end
end