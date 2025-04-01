require 'rails_helper'

RSpec.describe Api::V1::ExchangeRatesController, type: :controller do
  let(:czk) { Currency.new('CZK') }
  let(:usd) { Currency.new('USD') }
  let(:eur) { Currency.new('EUR') }

  let(:rates) do
    [
      ExchangeRate.new(from: czk, to: usd, rate: 23.117),
      ExchangeRate.new(from: czk, to: eur, rate: 24.930)
    ]
  end

  let(:service) { instance_double('RateService') }
  let(:provider) { instance_double('CNBProvider') }

  before do
    allow(controller).to receive(:exchange_rate_service).and_return(service)
    allow(service).to receive(:provider).and_return(provider)
    allow(provider).to receive(:metadata).and_return({
      source_name: "Czech National Bank (CNB)",
      base_currency: "CZK",
      supported_currencies: ['USD', 'EUR', 'JPY']
    })
    allow(provider).to receive(:supported_currencies).and_return(['USD', 'EUR', 'JPY'])

    # Default to no unavailable currencies
    allow(service).to receive(:unavailable_currencies).and_return({})
  end

  describe 'GET #index' do
    it 'returns all exchange rates when no currencies parameter' do
      allow(service).to receive(:get_rates).with(nil).and_return(rates)

      get :index

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      # Response should include rates array
      expect(json_response).to have_key('rates')
      expect(json_response['rates'].size).to eq(2)
      expect(json_response['rates'][0]['from']).to eq('CZK')
      expect(json_response['rates'][0]['to']).to eq('USD')
      expect(json_response['rates'][0]['rate']).to eq(23.117)

      # No warnings expected
      expect(json_response).not_to have_key('warnings')
    end

    it 'filters by currencies parameter' do
      allow(service).to receive(:get_rates).with(['USD']).and_return([rates.first])

      get :index, params: { currencies: 'USD' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)
      expect(json_response['rates'].size).to eq(1)
      expect(json_response['rates'][0]['to']).to eq('USD')
    end

    it 'handles comma-separated list of currencies' do
      allow(service).to receive(:get_rates).with(['USD', 'EUR']).and_return(rates)

      get :index, params: { currencies: 'USD,EUR' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)
      expect(json_response['rates'].size).to eq(2)
    end

    it 'strips whitespace from currency codes' do
      allow(service).to receive(:get_rates).with(['USD', 'EUR']).and_return(rates)

      get :index, params: { currencies: ' USD , EUR ' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)
      expect(json_response['rates'].size).to eq(2)
    end

    it 'includes warnings section when unavailable currencies are requested' do
      # Setup that GBP was requested but not available
      allow(service).to receive(:get_rates).with(['USD', 'GBP']).and_return([rates.first])
      allow(service).to receive(:unavailable_currencies).and_return({'GBP' => Time.now})

      get :index, params: { currencies: 'USD,GBP' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)

      # Should include rates section
      expect(json_response).to have_key('rates')
      expect(json_response['rates'].size).to eq(1)

      # Should include warnings section
      expect(json_response).to have_key('warnings')
      expect(json_response['warnings']).to have_key('unavailable_currencies')
      expect(json_response['warnings']['unavailable_currencies']).to include('GBP')

      # Should include provider info and available currencies
      expect(json_response['warnings']).to have_key('provider')
      expect(json_response['warnings']).to have_key('available_currencies')
      expect(json_response['warnings']['available_currencies']).to include('USD', 'EUR', 'JPY')
    end

    it 'does not include warnings when all requested currencies are available' do
      allow(service).to receive(:get_rates).with(['USD', 'EUR']).and_return(rates)

      get :index, params: { currencies: 'USD,EUR' }

      expect(response).to have_http_status(:success)
      json_response = JSON.parse(response.body)
      expect(json_response).not_to have_key('warnings')
    end

    it 'returns error status when service raises an exception' do
      allow(service).to receive(:get_rates).and_raise(StandardError.new('Test error'))

      get :index

      expect(response).to have_http_status(:internal_server_error)
      json_response = JSON.parse(response.body)
      expect(json_response).to have_key('error')
      expect(json_response['error']).to eq('Test error')
    end
  end
end