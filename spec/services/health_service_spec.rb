require 'rails_helper'

RSpec.describe HealthService do
  describe '#check_health' do
    let(:service) { described_class.new }
    let(:repository) { instance_double('Repository') }

    context 'with a repository that supports health metrics' do
      let(:metrics) do
        {
          redis_connected: true,
          cache_size: 100,
          cache_hit_ratio: 0.75,
          cache_hits: 75,
          cache_misses: 25,
          error_count: 5,
          fallback_size: 0
        }
      end

      before do
        allow(Rails.application.services).to receive(:get).with(:repository).and_return(repository)
        allow(repository).to receive(:respond_to?).with(:health_metrics).and_return(true)
        allow(repository).to receive(:health_metrics).and_return(metrics)
      end

      it 'returns health status with repository metrics' do
        result = service.check_health
        
        expect(result[:status]).to eq('ok')
        expect(result[:time]).to be_present
        expect(result[:redis]).to eq('connected')
        expect(result[:cache][:size]).to eq(100)
        expect(result[:cache][:hit_ratio]).to eq(0.75)
        expect(result[:errors][:count]).to eq(5)
        expect(result[:errors][:fallback_active]).to eq(false)
      end
    end

    context 'with a repository that does not support health metrics' do
      let(:redis) { instance_double('Redis') }

      before do
        allow(Rails.application.services).to receive(:get).with(:repository).and_return(repository)
        allow(repository).to receive(:respond_to?).with(:health_metrics).and_return(false)
        allow(Rails.application.config).to receive(:redis).and_return({})
        allow(Redis).to receive(:new).and_return(redis)
        allow(redis).to receive(:ping)
      end

      it 'uses basic Redis ping check' do
        result = service.check_health
        
        expect(result[:status]).to eq('ok')
        expect(result[:redis]).to eq('connected')
        expect(result).not_to have_key(:cache)
      end
    end

    context 'when repository is not available' do
      before do
        allow(Rails.application.services).to receive(:get).with(:repository).and_return(repository)
        allow(repository).to receive(:respond_to?).with(:health_metrics).and_raise(StandardError.new('Connection error'))
      end

      it 'reports Redis as disconnected' do
        result = service.check_health
        
        expect(result[:status]).to eq('ok')
        expect(result[:redis]).to eq('disconnected')
      end

      it 'includes error message in development' do
        allow(Rails.env).to receive(:development?).and_return(true)
        result = service.check_health
        
        expect(result[:error]).to eq('Connection error')
      end
    end
  end
end 