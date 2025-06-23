# frozen_string_literal: true

require 'rspec'
require 'webmock/rspec'
require_relative '../../lib/http_client'
require_relative '../../app/errors/provider_not_available_error'

describe HTTPClient do
  let(:url) { 'https://fakeapi.com/data' }
  let(:provider) { 'CZK' }

  context 'when the API responds successfully' do
    before do
      stub_request(:get, url).to_return(body: 'Fake API response')
    end

    it 'returns the response body' do
      expect(described_class.get(url, provider)).to eq('Fake API response')
    end
  end

  context 'when the API fails multiple times' do
    before do
      stub_request(:get, url).to_timeout.times(3)
    end

    it 'raises ProviderNotAvailableError after 3 failed attempts' do
      expect { described_class.get(url, provider) }
        .to raise_error(ProviderNotAvailableError, 'API from provider CZK is not available')
    end
  end
end
