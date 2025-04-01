require 'rails_helper'
require 'ostruct'

RSpec.describe ExchangeRateResponseFormatter do
  # Create a test controller to test the concern
  controller(ApplicationController) do
    include ExchangeRateResponseFormatter

    def index
      rate = OpenStruct.new(
        from: OpenStruct.new(code: 'USD'),
        to: OpenStruct.new(code: 'EUR'),
        rate: 0.93,
        date: Date.new(2023, 4, 1)
      )

      render json: format_rate(rate)
    end

    def show
      rate = OpenStruct.new(
        from: OpenStruct.new(code: 'USD'),
        to: OpenStruct.new(code: 'EUR'),
        rate: 0.93,
        date: Date.new(2023, 4, 1)
      )

      response = {
        base_currency: 'USD',
        rates: [format_rate(rate)]
      }

      add_warnings_to_response(response)

      render json: response
    end

    def exchange_rate_service
      @exchange_rate_service ||= OpenStruct.new(
        unavailable_currencies: { 'GBP' => Time.zone.now },
        provider: OpenStruct.new(
          supported_currencies: ['USD', 'EUR', 'JPY'],
          metadata: { source_name: 'Test Provider' }
        )
      )
    end
  end

  before do
    routes.draw do
      get 'index' => 'anonymous#index'
      get 'show' => 'anonymous#show'
    end
  end

  describe '#format_rate' do
    it 'correctly formats an exchange rate' do
      get :index
      expect(response).to have_http_status(:success)
      json_response = response.parsed_body

      expect(json_response['from']).to eq('USD')
      expect(json_response['to']).to eq('EUR')
      expect(json_response['rate']).to eq(0.93)
      expect(json_response['date']).to eq('2023-04-01')
    end
  end

  describe '#add_warnings_to_response' do
    it 'adds warnings about unavailable currencies' do
      get :show
      expect(response).to have_http_status(:success)
      json_response = response.parsed_body

      expect(json_response).to have_key('warnings')
      expect(json_response['warnings']['unavailable_currencies']).to include('GBP')
      expect(json_response['warnings']['available_currencies']).to include('USD', 'EUR', 'JPY')
      expect(json_response['warnings']['provider']).to eq('Test Provider')
    end
  end
end