require 'rails_helper'

RSpec.describe RedisExchangeRateRepository do
  let(:redis_mock) { instance_double(Redis) }
  let(:connection_pool) { instance_double(ConnectionPool) }
  let(:repository) { RedisExchangeRateRepository.new(redis_mock) }
  
  let(:date) { Date.new(2024, 1, 1) }
  let(:rates) do
    [
      ExchangeRate.new(from: 'USD', to: 'EUR', rate: 0.92, date: date),
      ExchangeRate.new(from: 'USD', to: 'GBP', rate: 0.77, date: date),
      ExchangeRate.new(from: 'USD', to: 'JPY', rate: 150.12, date: date)
    ]
  end
  
  let(:packed_rates) do
    rates_data = rates.map do |rate|
      {
        from: rate.from.code,
        to: rate.to.code,
        rate: rate.rate,
        date: date.to_s
      }
    end
    MessagePack.pack(rates_data)
  end
  
  let(:json_rates) do
    rates.map do |rate|
      {
        from: rate.from.code,
        to: rate.to.code,
        rate: rate.rate,
        date: date.to_s
      }
    end.to_json
  end
  
  let(:metadata) { { cached_at: Time.new(2024, 1, 1, 12, 0, 0).to_s } }
  let(:serialized_metadata) { metadata.to_json }
  
  before do
    # Setup connection pooling mock
    allow(redis_mock).to receive(:is_a?).with(ConnectionPool).and_return(false)
  end
  
  describe '#fetch_for' do
    it 'returns nil when no data exists for the date' do
      allow(redis_mock).to receive(:get).with("exchange_rates:rates:#{date}").and_return(nil)
      
      expect(repository.fetch_for(date)).to be_nil
      expect(repository.cache_misses).to eq(1)
      expect(repository.cache_hits).to eq(0)
    end
    
    it 'returns deserialized rates when data exists (MessagePack)' do
      allow(redis_mock).to receive(:get).with("exchange_rates:rates:#{date}").and_return(packed_rates)
      
      fetched_rates = repository.fetch_for(date)
      expect(fetched_rates.size).to eq(3)
      expect(fetched_rates.first.from.code).to eq('USD')
      expect(fetched_rates.first.to.code).to eq('EUR')
      expect(fetched_rates.first.rate).to eq(0.92)
      
      expect(repository.cache_hits).to eq(1)
      expect(repository.cache_misses).to eq(0)
    end
    
    it 'returns deserialized rates when data exists (JSON for backward compatibility)' do
      allow(redis_mock).to receive(:get).with("exchange_rates:rates:#{date}").and_return(json_rates)
      
      fetched_rates = repository.fetch_for(date)
      expect(fetched_rates.size).to eq(3)
      expect(fetched_rates.first.from.code).to eq('USD')
      expect(fetched_rates.first.to.code).to eq('EUR')
      expect(fetched_rates.first.rate).to eq(0.92)
    end
    
    it 'falls back to in-memory cache when Redis fails' do
      # First Redis succeeds
      allow(redis_mock).to receive(:get).with("exchange_rates:rates:#{date}").and_return(packed_rates)
      
      # Get the rates initially - they will be stored in the fallback cache
      initial_rates = repository.fetch_for(date)
      expect(initial_rates.size).to eq(3)
      
      # Now Redis fails
      allow(redis_mock).to receive(:get).with("exchange_rates:rates:#{date}")
        .and_raise(Redis::CannotConnectError.new("Connection failed"))
      
      # Should return from fallback cache
      fetched_rates = repository.fetch_for(date)
      expect(fetched_rates.size).to eq(3)
      expect(fetched_rates.first.from.code).to eq('USD')
      expect(repository.errors).to eq(1)
    end
  end
  
  describe '#save_for' do
    it 'serializes and stores rates with TTL using MessagePack' do
      expect(redis_mock).to receive(:setex).with(
        "exchange_rates:rates:#{date}", 
        RedisExchangeRateRepository::DEFAULT_TTL, 
        packed_rates
      )
      
      expect(redis_mock).to receive(:setex).with(
        "exchange_rates:metadata:#{date}",
        RedisExchangeRateRepository::DEFAULT_TTL,
        instance_of(String)
      )
      
      saved_rates = repository.save_for(date, rates)
      expect(saved_rates).to eq(rates)
    end
    
    it 'updates fallback cache when Redis fails' do
      allow(redis_mock).to receive(:setex)
        .and_raise(Redis::CannotConnectError.new("Connection failed"))
      
      repository.save_for(date, rates)
      
      # Try to fetch (should come from fallback cache)
      allow(redis_mock).to receive(:get)
        .and_raise(Redis::CannotConnectError.new("Connection failed"))
      
      fetched = repository.fetch_for(date)
      expect(fetched.size).to eq(rates.size)
      expect(repository.errors).to eq(2)
    end
  end
  
  describe '#metadata_for' do
    it 'returns nil when no metadata exists' do
      allow(redis_mock).to receive(:get).with("exchange_rates:metadata:#{date}").and_return(nil)
      
      expect(repository.metadata_for(date)).to be_nil
    end
    
    it 'returns deserialized metadata when it exists' do
      allow(redis_mock).to receive(:get).with("exchange_rates:metadata:#{date}").and_return(serialized_metadata)
      
      meta = repository.metadata_for(date)
      expect(meta).to include(cached_at: metadata[:cached_at])
    end
  end
  
  describe '#clear' do
    it 'deletes both data and metadata keys' do
      expect(redis_mock).to receive(:del).with(
        "exchange_rates:rates:#{date}",
        "exchange_rates:metadata:#{date}"
      )
      
      repository.clear(date)
    end
    
    it 'clears fallback cache too' do
      # First save to put in fallback cache
      allow(redis_mock).to receive(:setex)
      repository.save_for(date, rates)
      
      # Then clear
      allow(redis_mock).to receive(:del)
      repository.clear(date)
      
      # Force Redis failure to test fallback
      allow(redis_mock).to receive(:get)
        .and_raise(Redis::CannotConnectError.new("Connection failed"))
      
      # Should not be in fallback anymore
      expect(repository.fetch_for(date)).to be_nil
    end
  end
  
  describe '#health_metrics' do
    before do
      allow(repository).to receive(:redis_connected?).and_return(true)
      allow(repository).to receive(:cached_dates).and_return([date])
      
      # Add some test metrics
      repository.instance_variable_set(:@cache_hits, 5)
      repository.instance_variable_set(:@cache_misses, 2)
      repository.instance_variable_set(:@errors, 1)
    end
    
    it 'returns complete metrics hash' do
      metrics = repository.health_metrics
      
      expect(metrics[:redis_connected]).to eq(true)
      expect(metrics[:cache_size]).to eq(1)
      expect(metrics[:cache_hit_ratio]).to be_within(0.01).of(0.71)
      expect(metrics[:cache_hits]).to eq(5)
      expect(metrics[:cache_misses]).to eq(2)
      expect(metrics[:error_count]).to eq(1)
    end
  end
  
  describe 'connection pooling' do
    let(:pool_repository) { RedisExchangeRateRepository.new(connection_pool) }
    
    before do
      allow(connection_pool).to receive(:is_a?).with(ConnectionPool).and_return(true)
      allow(connection_pool).to receive(:with).and_yield(redis_mock)
    end
    
    it 'uses pool for fetching' do
      expect(connection_pool).to receive(:with).and_yield(redis_mock)
      allow(redis_mock).to receive(:get).with("exchange_rates:rates:#{date}").and_return(packed_rates)
      
      rates = pool_repository.fetch_for(date)
      expect(rates.size).to eq(3)
    end
    
    it 'uses pool for saving' do
      expect(connection_pool).to receive(:with).and_yield(redis_mock)
      allow(redis_mock).to receive(:setex)
      
      pool_repository.save_for(date, rates)
    end
  end
end 