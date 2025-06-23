# frozen_string_literal: true

require 'rspec'
require 'rack/test'
require_relative '../../../app/controllers/api/exchange_rates_controller'
require_relative '../../../app/services/exchange_rate_service'

describe Api::ExchangeRatesController do
  context 'when a valid source is provided' do
    before do
      allow(ExchangeRateService).to receive(:get_rates).with('CZK', []).and_return({
        'CZK': [
          { "currency_code": 'EUR', "rate": 25.065, "currency": 'Euro' },
          { "currency_code": 'USD', "rate": 24.282, "currency": 'Dolar' },
          { "currency_code": 'GBP', "rate": 30.093, "currency": 'Libra' }
        ]
      })
    end

    it 'returns exchange rates' do
      get '/api/exchange_rates', { source: 'CZK' }

      expect(last_response.status).to eq(200)
      expect(JSON.parse(last_response.body)).to eq({
        'rates' => {
          'CZK' => [
            { 'currency_code' => 'EUR', 'rate' => 25.065, 'currency' => 'Euro' },
            { 'currency_code' => 'USD', 'rate' => 24.282, 'currency' => 'Dolar' },
            { 'currency_code' => 'GBP', 'rate' => 30.093, 'currency' => 'Libra' }
          ]
        }
      })
    end
  end

  context 'when currency filter is provided' do
    before do
      allow(ExchangeRateService).to receive(:get_rates).with('CZK', ['EUR', 'USD']).and_return({
        'CZK': [
          { "currency_code": 'EUR', "rate": 25.065, "currency": 'Euro' },
          { "currency_code": 'USD', "rate": 24.282, "currency": 'Dolar' }
        ]
      })
    end

    it 'returns only the requested exchange rates' do
      get '/api/exchange_rates', { source: 'CZK', currencies: 'EUR,USD' }

      expect(last_response.status).to eq(200)
      expect(JSON.parse(last_response.body)).to eq({
        'rates' => {
          'CZK' => [
            { 'currency_code' => 'EUR', 'rate' => 25.065, 'currency' => 'Euro' },
            { 'currency_code' => 'USD', 'rate' => 24.282, 'currency' => 'Dolar' }
          ]
        }
      })
    end
  end

  context 'when an unsupported source is provided' do
    before do
      allow(ExchangeRateService).to receive(:get_rates).with('GB', [])
                                                       .and_raise(UnsupportedProviderError.new('GB'))
    end

    it 'returns an error response' do
      get '/api/exchange_rates', source: 'GB'

      expect(last_response.status).to eq(422)
      expect(JSON.parse(last_response.body)).to eq({
        'error' => 'No provider available for bank from GB'
      })
    end
  end

  context 'when the external API Bank Provider is down' do
    before do
      allow(ExchangeRateService).to receive(:get_rates)
        .with('CZK', [])
        .and_raise(ProviderNotAvailableError, 'CZK')
    end

    it 'returns 503 Service Unavailable' do
      get '/api/exchange_rates', { source: 'CZK' }

      expect(last_response.status).to eq(503)
      expect(JSON.parse(last_response.body)).to eq({
        'error' => 'Service unavailable: API from provider CZK is not available'
      })
    end
  end
end
