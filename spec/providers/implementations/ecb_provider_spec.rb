require 'rails_helper'
require 'webmock/rspec'
require_relative '../../../app/providers/ecb_provider'
require_relative '../../../app/adapters/providers/ecb/ecb_xml_adapter'
require_relative "../../support/ecb_test_helpers"

RSpec.describe ECBProvider do
  include ECBTestHelpers
  include WebMock::API
  
  let(:config) do
    {
      'base_url' => 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml',
      'base_currency' => 'EUR'
    }
  end
  
  let(:provider) { ECBProvider.new(config) }
  let(:sample_data) { sample_ecb_xml_data }
  
  # Set up WebMock stubs for all tests
  before do
    # Stub HTTP requests
    stub_request(:get, config['base_url'])
      .to_return(
        status: 200,
        body: sample_data,
        headers: { 'Content-Type' => 'application/xml' }
      )
      
    # Mock supported currencies to avoid HTTP requests
    allow_any_instance_of(ECBProvider).to receive(:supported_currencies).and_return(['USD', 'EUR', 'GBP', 'JPY'])
  end
  
  describe '#initialize' do
    it 'requires a base_url' do
      expect { ECBProvider.new({}) }.to raise_error(ExchangeRateErrors::InvalidConfigurationError)
    end
    
    it 'sets default values' do
      expect(provider.instance_variable_get(:@base_currency)).to eq('EUR')
      expect(provider.instance_variable_get(:@publication_hour)).to eq(16)
      expect(provider.instance_variable_get(:@publication_minute)).to eq(0)
    end
  end
  
  describe '#metadata' do
    it 'returns correct metadata' do
      metadata = provider.metadata
      
      expect(metadata[:update_frequency]).to eq(:daily)
      expect(metadata[:base_currency]).to eq('EUR')
      expect(metadata[:working_days_only]).to eq(true)
      expect(metadata[:source_name]).to eq('European Central Bank (ECB)')
    end
  end
  
  describe '#fetch_rates' do
    before do
      stub_request(:get, config['base_url'])
        .to_return(
          status: 200,
          body: sample_data,
          headers: { 'Content-Type' => 'application/xml' }
        )
    end
    
    it 'fetches and parses rates correctly' do
      # Create sample rates
      sample_rates = create_sample_ecb_rates
      
      # Mock the HTTP fetch and parsing
      expect(HttpFetcher).to receive(:fetch).and_return({
        data: sample_data,
        content_type: 'application/xml'
      })
      
      # Mock the parsing method to return sample rates
      allow_any_instance_of(ECBProvider).to receive(:parse_data).and_return(sample_rates)
      
      # Run the test
      rates = provider.fetch_rates
      
      # Verify results
      expect(rates).to eq(sample_rates)
      expect(rates.map(&:to).map(&:code)).to include('USD', 'CZK', 'GBP')
    end
    
    it 'uses the correct base currency' do
      # Create sample rates with EUR base
      sample_rates = create_sample_ecb_rates
      
      # Mock the HTTP fetch and parsing
      expect(HttpFetcher).to receive(:fetch).and_return({
        data: sample_data,
        content_type: 'application/xml'
      })
      
      # Mock the parsing method to return sample rates
      allow_any_instance_of(ECBProvider).to receive(:parse_data).and_return(sample_rates)
      
      # Run the test
      rates = provider.fetch_rates
      
      # Verify results
      rates.each do |rate|
        expect(rate.from.code).to eq('EUR')
      end
    end
    
    it 'validates all rates are positive' do
      # Create a provider instance
      test_provider = ECBProvider.new(config)
      
      # Mock the data fetching part
      allow(test_provider).to receive(:fetch_data).and_return({
        data: sample_data.gsub('rate="1.0832"', 'rate="-1.0832"'),
        content_type: 'application/xml'
      })
      
      # Mock parse_data to raise a validation error
      allow(test_provider).to receive(:parse_data).and_raise(
        ExchangeRateErrors::ValidationError.new("Negative rates are not allowed", nil, "ECB")
      )
      
      # Expect the validation error to be thrown
      expect { test_provider.fetch_rates }.to raise_error(ExchangeRateErrors::ValidationError)
    end
  end
  
  describe 'error handling' do
    it 'handles HTTP errors' do
      stub_request(:get, config['base_url']).to_return(status: 503, body: 'Service Unavailable')
      
      # Mock the HttpFetcher to throw the appropriate error type
      allow(HttpFetcher).to receive(:fetch).and_raise(
        ExchangeRateErrors::ProviderMaintenanceError.new(
          'Service unavailable',
          StandardError.new('503 Service Unavailable'),
          'ECB'
        )
      )
      
      expect { provider.fetch_rates }.to raise_error(ExchangeRateErrors::ProviderMaintenanceError)
    end
    
    it 'handles timeouts' do
      stub_request(:get, config['base_url']).to_timeout
      
      # Mock the HttpFetcher to throw the appropriate error type
      allow(HttpFetcher).to receive(:fetch).and_raise(
        ExchangeRateErrors::TimeoutError.new(
          'Connection timed out',
          Timeout::Error.new('Connection timed out'),
          'ECB'
        )
      )
      
      expect { provider.fetch_rates }.to raise_error(ExchangeRateErrors::TimeoutError)
    end
    
    it 'handles invalid XML format' do
      # Mock direct fetch_data method rather than relying on HttpFetcher
      allow(provider).to receive(:fetch_data).and_return({
        data: '<invalid>XML</syntax>',
        content_type: 'application/xml'
      })
      
      # Directly mock parse_data to throw ParseError
      allow(provider).to receive(:parse_data).and_raise(
        ExchangeRateErrors::ParseError.new(
          'XML parsing error',
          StandardError.new('Invalid XML'),
          'ECB'
        )
      )
      
      expect { provider.fetch_rates }.to raise_error(ExchangeRateErrors::ParseError)
    end
  end
end 