require 'rails_helper'

RSpec.describe ExchangeRateErrorHandler do
  # Create a test controller to test the concern
  controller(ApplicationController) do
    include ExchangeRateErrorHandler

    def index
      raise ExchangeRateErrors::ValidationError, "Invalid currency"
    end

    def show
      raise ExchangeRateErrors::ProviderAuthenticationError, "Auth failed"
    end

    def update
      raise ExchangeRateErrors::ProviderUnavailableError, "Provider down for maintenance"
    end
  end

  before do
    routes.draw do
      get 'index' => 'anonymous#index'
      get 'show' => 'anonymous#show'
      get 'update' => 'anonymous#update'
    end
  end

  describe 'exchange rate error handling' do
    it 'maps ValidationError to 400 status code' do
      get :index
      expect(response).to have_http_status(:bad_request)
      json_response = response.parsed_body
      expect(json_response['error']['type']).to eq('ValidationError')
      expect(json_response['error']['message']).to eq('Invalid currency')
      expect(json_response['error']['code']).to eq('bad_request')
    end

    it 'maps ProviderAuthenticationError to 401 status code' do
      get :show
      expect(response).to have_http_status(:unauthorized)
      json_response = response.parsed_body
      expect(json_response['error']['type']).to eq('ProviderAuthenticationError')
      expect(json_response['error']['code']).to eq('unauthorized')
    end

    it 'maps ProviderUnavailableError to 503 status code' do
      get :update
      expect(response).to have_http_status(:service_unavailable)
      json_response = response.parsed_body
      expect(json_response['error']['type']).to eq('ProviderUnavailableError')
      expect(json_response['error']['code']).to eq('service_unavailable')
    end
  end
end