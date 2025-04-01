require 'rails_helper'

RSpec.describe ErrorHandler, type: :controller do
  # Create a test controller to test the concern
  controller(ApplicationController) do
    include ErrorHandler

    def index
      raise StandardError, "Test error"
    end
  end

  describe 'error handling' do
    context 'in test environment' do
      before do
        allow(Rails.env).to receive(:test?).and_return(true)
      end

      it 'returns simplified error format' do
        get :index
        expect(response).to have_http_status(:internal_server_error)
        json_response = JSON.parse(response.body)
        expect(json_response).to have_key('error')
        expect(json_response['error']).to eq('Test error')
      end
    end

    context 'in non-test environment' do
      before do
        allow(Rails.env).to receive(:test?).and_return(false)
        allow(Rails.env).to receive(:production?).and_return(false)
      end

      it 'returns detailed error response with debug info' do
        get :index
        expect(response).to have_http_status(:internal_server_error)
        json_response = JSON.parse(response.body)
        expect(json_response).to have_key('error')
        expect(json_response['error']['type']).to eq('InternalServerError')
        expect(json_response['error']['message']).to eq('Test error')
        expect(json_response['error']['code']).to eq('internal_server_error')
        expect(json_response['error']).to have_key('backtrace')
      end
    end

    context 'in production environment' do
      before do
        allow(Rails.env).to receive(:test?).and_return(false)
        allow(Rails.env).to receive(:production?).and_return(true)
      end

      it 'returns error response without debug info' do
        get :index
        expect(response).to have_http_status(:internal_server_error)
        json_response = JSON.parse(response.body)
        expect(json_response).to have_key('error')
        expect(json_response['error']['type']).to eq('InternalServerError')
        expect(json_response['error']['message']).to eq('Test error')
        expect(json_response['error']['code']).to eq('internal_server_error')
        expect(json_response['error']).not_to have_key('backtrace')
      end
    end
  end
end 