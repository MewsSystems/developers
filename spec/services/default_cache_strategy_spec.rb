require 'spec_helper'

RSpec.describe DefaultCacheStrategy do
  let(:provider) { MockProvider.new('base_currency' => 'USD') }
  let(:repository) { MockRepository.new }
  let(:strategy) { DefaultCacheStrategy.new(provider, repository) }
  let(:sample_rates) { create_sample_rates }
  let(:today) { Date.today }

  before do
    provider.set_rates(sample_rates)
  end

  # Helper method to create sample rates for testing
  def create_sample_rates(base_code = 'USD', date = Date.today)
    [
      ExchangeRate.new(from: Currency.new(base_code), to: Currency.new('EUR'), rate: 0.85, date: date),
      ExchangeRate.new(from: Currency.new(base_code), to: Currency.new('GBP'), rate: 0.75, date: date),
      ExchangeRate.new(from: Currency.new(base_code), to: Currency.new('JPY'), rate: 110.0, date: date)
    ]
  end

  describe '#determine_fetch_date' do
    context 'with daily update frequency' do
      before do
        allow(provider).to receive(:metadata).and_return({
          update_frequency: :daily,
          publication_time: Time.new(today.year, today.month, today.day, 14, 30, 0),
          working_days_only: true
        })
      end

      it 'returns today if current time is after publication time' do
        allow(Time).to receive(:now).and_return(Time.new(today.year, today.month, today.day, 15, 0, 0))
        expect(strategy.determine_fetch_date).to eq(today)
      end

      it 'returns previous business day if current time is before publication time' do
        allow(Time).to receive(:now).and_return(Time.new(today.year, today.month, today.day, 14, 0, 0))

        # If today is Monday, previous business day is Friday (3 days ago)
        if today.monday?
          expect(strategy.determine_fetch_date).to eq(today - 3)
        # If today is Sunday, previous business day is Friday (2 days ago)
        elsif today.sunday?
          expect(strategy.determine_fetch_date).to eq(today - 2)
        # If today is Saturday, previous business day is Friday (1 day ago)
        elsif today.saturday?
          expect(strategy.determine_fetch_date).to eq(today - 1)
        # Otherwise, previous business day is yesterday
        else
          expect(strategy.determine_fetch_date).to eq(today - 1)
        end
      end
    end

    context 'with hourly update frequency' do
      before do
        allow(provider).to receive(:metadata).and_return({
          update_frequency: :hourly
        })
      end

      it 'always returns today' do
        expect(strategy.determine_fetch_date).to eq(today)
      end
    end

    context 'with missing metadata' do
      before do
        allow(provider).to receive(:metadata).and_return(nil)
      end

      it 'falls back to today' do
        expect(strategy.determine_fetch_date).to eq(today)
      end
    end
  end

  describe '#cache_fresh?' do
    let(:fetch_date) { today }

    context 'when cache is fresh' do
      before do
        allow(repository).to receive(:cache_time_for).with(fetch_date).and_return(Time.now - 300) # 5 minutes ago
        allow(strategy).to receive(:calculate_cache_ttl).and_return(3600) # 1 hour TTL
      end

      it 'returns true' do
        expect(strategy.cache_fresh?(fetch_date)).to be true
      end
    end

    context 'when cache is stale' do
      before do
        allow(repository).to receive(:cache_time_for).with(fetch_date).and_return(Time.now - 7200) # 2 hours ago
        allow(strategy).to receive(:calculate_cache_ttl).and_return(3600) # 1 hour TTL
      end

      it 'returns false' do
        expect(strategy.cache_fresh?(fetch_date)).to be false
      end
    end

    context 'when cache time is not available' do
      before do
        allow(repository).to receive(:cache_time_for).with(fetch_date).and_return(nil)
      end

      it 'returns false' do
        expect(strategy.cache_fresh?(fetch_date)).to be false
      end
    end
  end

  describe '#calculate_cache_ttl' do
    context 'with daily update frequency' do
      before do
        allow(provider).to receive(:metadata).and_return({
          update_frequency: :daily,
          publication_time: Time.new(today.year, today.month, today.day, 14, 30, 0)
        })
      end

      it 'returns a TTL based on time until next publication' do
        # Mock next publication time calculation
        allow(strategy).to receive(:calculate_next_publication).and_return(Time.now + 3600) # 1 hour from now

        # Should return approximately 1 hour (with a small margin)
        ttl = strategy.calculate_cache_ttl
        expect(ttl).to be_within(600).of(3600) # Within 10 minutes of 1 hour
      end
    end

    context 'with hourly update frequency' do
      before do
        allow(provider).to receive(:metadata).and_return({
          update_frequency: :hourly
        })
      end

      it 'returns 15 minutes' do
        expect(strategy.calculate_cache_ttl).to eq(15 * 60)
      end
    end

    context 'with realtime update frequency' do
      before do
        allow(provider).to receive(:metadata).and_return({
          update_frequency: :realtime
        })
      end

      it 'returns 30 seconds' do
        expect(strategy.calculate_cache_ttl).to eq(30)
      end
    end

    context 'with missing metadata' do
      before do
        allow(provider).to receive(:metadata).and_return(nil)
      end

      it 'returns default TTL of 1 hour' do
        expect(strategy.calculate_cache_ttl).to eq(3600)
      end
    end
  end

  describe '#get_cached_rates' do
    let(:fetch_date) { today }

    context 'when cache is fresh' do
      before do
        allow(repository).to receive(:fetch_for).with(fetch_date).and_return(sample_rates)
        allow(strategy).to receive(:cache_fresh?).with(fetch_date).and_return(true)
      end

      it 'returns cached rates' do
        expect(strategy.get_cached_rates(fetch_date)).to eq(sample_rates)
      end
    end

    context 'when cache is stale' do
      before do
        allow(repository).to receive(:fetch_for).with(fetch_date).and_return(sample_rates)
        allow(strategy).to receive(:cache_fresh?).with(fetch_date).and_return(false)
      end

      it 'returns nil to trigger refresh' do
        expect(strategy.get_cached_rates(fetch_date)).to be_nil
      end
    end

    context 'when cache is empty' do
      before do
        allow(repository).to receive(:fetch_for).with(fetch_date).and_return(nil)
      end

      it 'returns nil to trigger fetch' do
        expect(strategy.get_cached_rates(fetch_date)).to be_nil
      end
    end

    context 'when force refresh is requested' do
      before do
        allow(repository).to receive(:fetch_for).with(fetch_date).and_return(sample_rates)
        allow(strategy).to receive(:cache_fresh?).with(fetch_date).and_return(true)
      end

      it 'returns nil even if cache is fresh' do
        expect(strategy.get_cached_rates(fetch_date, true)).to be_nil
      end
    end
  end

  describe '#handle_fetch_error' do
    let(:fetch_date) { today }
    let(:stale_rates) { create_sample_rates('USD', today - 1) }

    context 'with provider unavailable error' do
      let(:error) { ExchangeRateErrors::ProviderUnavailableError.new("Service unavailable") }

      context 'when stale data is available' do
        before do
          allow(repository).to receive(:fetch_for).with(fetch_date, allow_stale: true).and_return(stale_rates)
        end

        it 'returns stale data' do
          expect(strategy.handle_fetch_error(error, fetch_date)).to eq(stale_rates)
        end
      end

      context 'when no stale data is available' do
        before do
          allow(repository).to receive(:fetch_for).with(fetch_date, allow_stale: true).and_return(nil)
        end

        it 're-raises the error' do
          expect do
            strategy.handle_fetch_error(error, fetch_date)
          end.to raise_error(ExchangeRateErrors::ProviderUnavailableError)
        end
      end
    end

    context 'with other errors' do
      let(:error) { ExchangeRateErrors::ValidationError.new("Invalid data") }

      it 're-raises the error' do
        expect { strategy.handle_fetch_error(error, fetch_date) }.to raise_error(ExchangeRateErrors::ValidationError)
      end
    end
  end
end