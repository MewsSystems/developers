require 'rails_helper'

# Simple test implementation of BaseProvider
class TestProvider < BaseProvider
  attr_accessor :test_metadata

  def initialize(metadata = {})
    # Add required base_url for provider config
    test_config = { 'base_url' => 'https://test-provider.example.com/api' }

    super(test_config)

    @test_metadata = metadata
  end

  def fetch_rates
    []
  end

  def metadata
    @test_metadata
  end

  # Support validation tests
  def validate_provider_specific_rates(rates)
    # No specific validation needed for tests
  end
end

RSpec.describe BaseProvider do
  let(:provider) { BaseProvider.new('base_url' => 'https://example.com/api') }
  let(:test_provider) { TestProvider.new }

  describe "concern inclusion" do
    it "includes all provider concerns" do
      expect(BaseProvider.included_modules).to include(ExchangeRateProviderInterface)
      expect(BaseProvider.included_modules).to include(LoggingHelper)
      expect(BaseProvider.included_modules).to include(ProviderValidation)
      expect(BaseProvider.included_modules).to include(ProviderCache)
      expect(BaseProvider.included_modules).to include(ProviderOperations)
      expect(BaseProvider.included_modules).to include(ProviderDateTime)
      expect(BaseProvider.included_modules).to include(ProviderConfig)
      expect(BaseProvider.included_modules).to include(ProviderDataFetching)
    end
  end

  describe "#initialize" do
    it "sets up provider name" do
      expect(provider.instance_variable_get(:@provider_name)).to eq("Base")
    end

    it "applies configuration" do
      allow(Utils::ProviderConfig).to receive(:configure).and_return({
        base_currency: "USD",
        publication_hour: 14,
        publication_minute: 30,
        publication_timezone: "+01:00"
      })

      provider = BaseProvider.new('base_url' => 'https://example.com/api')

      expect(Utils::ProviderConfig).to have_received(:configure).with("Base",
                                                                      { 'base_url' => 'https://example.com/api' })
      expect(provider.instance_variable_get(:@base_currency)).to eq("USD")
      expect(provider.instance_variable_get(:@publication_hour)).to eq(14)
      expect(provider.instance_variable_get(:@publication_minute)).to eq(30)
      expect(provider.instance_variable_get(:@publication_timezone)).to eq("+01:00")
    end

    it "extracts file extension from URL" do
      provider = BaseProvider.new('base_url' => 'https://example.com/api.xml')
      expect(provider.instance_variable_get(:@file_extension)).to eq(".xml")
    end

    it "initializes metadata using setup_provider_metadata" do
      metadata = { source_name: "Test" }
      allow_any_instance_of(BaseProvider).to receive(:setup_provider_metadata).and_return(metadata)

      provider = BaseProvider.new('base_url' => 'https://example.com/api')

      expect(provider.instance_variable_get(:@metadata)).to eq(metadata)
    end
  end

  describe "#fetch_rates" do
    it "raises NotImplementedError when called directly on BaseProvider" do
      expect { provider.fetch_rates }.to raise_error(NotImplementedError, /must implement fetch_rates/)
    end
  end

  describe "#metadata" do
    it "raises NotImplementedError when called directly on BaseProvider" do
      expect { provider.metadata }.to raise_error(NotImplementedError, /must implement metadata/)
    end
  end

  describe "#supported_currencies" do
    it "returns cached supported_currencies if available" do
      test_provider.instance_variable_set(:@supported_currencies, ["USD", "EUR"])
      expect(test_provider.supported_currencies).to eq(["USD", "EUR"])
    end

    it "calls fetch_supported_currencies if no cached currencies" do
      test_provider.instance_variable_set(:@supported_currencies, nil)
      allow(test_provider).to receive(:fetch_supported_currencies).and_return(["USD", "EUR"])

      result = test_provider.supported_currencies

      expect(result).to eq(["USD", "EUR"])
      expect(test_provider).to have_received(:fetch_supported_currencies)
    end
  end
end