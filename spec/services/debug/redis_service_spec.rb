require 'rails_helper'

RSpec.describe Debug::RedisService do
  describe '#test_operations' do
    let(:service) { described_class.new }
    let(:repository) { double('Repository') }
    let(:test_date) { Date.today }
    let(:test_rate) { ExchangeRate.new(from: 'USD', to: 'EUR', rate: 0.93, date: test_date) }
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
      allow(Date).to receive(:today).and_return(test_date)

      # Mock ExchangeRate class
      allow(ExchangeRate).to receive(:new).and_return(test_rate)

      # Mock repository operations
      allow(repository).to receive(:save_for).with(test_date, [test_rate])
      allow(repository).to receive(:fetch_for).with(test_date).and_return([test_rate])
      allow(repository).to receive(:clear).with(test_date)
      allow(repository).to receive_messages(health_metrics: metrics, class: double('Class', name: 'RedisRepository'))
    end

    it 'returns repository information and test results' do
      result = service.test_operations

      expect(result[:repository_class]).to eq('RedisRepository')
      expect(result[:metrics]).to eq(metrics)
      expect(result[:test_results][:save_successful]).to be(true)
      expect(result[:test_results][:fetch_count]).to eq(1)
      expect(result[:test_results][:rates].first).to have_key(:from)
    end

    it 'performs Redis operations in the correct sequence' do
      expect(repository).to receive(:save_for).with(test_date, [test_rate]).ordered
      expect(repository).to receive(:fetch_for).with(test_date).ordered
      expect(repository).to receive(:clear).with(test_date).ordered
      expect(repository).to receive(:fetch_for).with(test_date).ordered
      expect(repository).to receive(:health_metrics).ordered

      service.test_operations
    end
  end
end