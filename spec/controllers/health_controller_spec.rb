require 'rails_helper'

RSpec.describe HealthController do
  describe 'GET #index' do
    before do
      routes.draw do
        get 'health' => 'health#index'
      end
      allow(HealthService).to receive(:new).and_return(health_service)
      allow(health_service).to receive(:check_health).and_return(health_data)
    end

    let(:health_service) { instance_double(HealthService) }
    let(:health_data) { { status: 'ok', time: '2023-04-01T12:00:00Z', redis: 'connected' } }

    it 'returns health status from the health service' do
      get :index
      expect(response).to have_http_status(:success)
      json_response = response.parsed_body
      expect(json_response['status']).to eq('ok')
      expect(json_response['redis']).to eq('connected')
    end
  end
end