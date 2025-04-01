require 'rails_helper'

RSpec.describe Debug::RedisController do
  describe 'GET #index' do
    # Set up routes for the test
    before do
      routes.draw do
        namespace :debug do
          get 'redis' => 'redis#index'
        end
      end
      allow(Debug::RedisService).to receive(:new).and_return(redis_service)
      allow(redis_service).to receive(:test_operations).and_return(test_data)
    end

    let(:redis_service) { instance_double(Debug::RedisService) }
    let(:test_data) do
      {
        repository_class: 'RedisRepository',
        metrics: { redis_connected: true, cache_size: 10 },
        test_results: {
          save_successful: true,
          fetch_count: 1,
          rates: [{ from: 'USD', to: 'EUR', rate: 0.93 }]
        }
      }
    end

    context 'in development environment' do
      before do
        allow(Rails.env).to receive(:development?).and_return(true)
      end

      it 'returns Redis debug data from the service' do
        get :index
        expect(response).to have_http_status(:success)
        json_response = response.parsed_body
        expect(json_response['repository_class']).to eq('RedisRepository')
        expect(json_response['test_results']['save_successful']).to be(true)
      end
    end

    context 'in non-development environment' do
      before do
        allow(Rails.env).to receive(:development?).and_return(false)
      end

      it 'returns forbidden status' do
        get :index
        expect(response).to have_http_status(:forbidden)
      end
    end
  end
end