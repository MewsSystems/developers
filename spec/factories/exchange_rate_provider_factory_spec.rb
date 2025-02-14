# frozen_string_literal: true

require_relative '../../app/providers/exchange_rate_provider'
require_relative '../../app/providers/czech_exchange_rate_provider'
require_relative '../../app/factories/exchange_rate_provider_factory'
require_relative '../../app/errors/unsupported_provider_error'

describe ExchangeRateProviderFactory do
  context 'when a valid source is provided' do
    it 'returns an instance of CzechExchangeRateProvider for CZK' do
      provider = described_class.create('CZK')
      expect(provider).to be_an_instance_of(CzechExchangeRateProvider)
    end
  end

  context 'when an invalid source is provided' do
    it 'raises an error for unsupported currencies' do
      expect { described_class.create('GB') }
        .to raise_error(UnsupportedProviderError, 'No provider available for bank from GB')
    end
  end
end
