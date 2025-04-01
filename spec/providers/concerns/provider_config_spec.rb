require 'rails_helper'
require_relative 'shared_examples'

RSpec.describe ProviderConfig do
  include_examples "a provider concern"

  let(:provider) { TestProvider.new }
  let(:provider_name) { "TestProvider" }

  describe "#setup_provider_metadata" do
    it "sets up metadata with default values" do
      config = {
        base_currency: "USD"
      }
      
      allow(Utils::ProviderConfig).to receive(:build_metadata).and_return({ source_name: provider_name })
      
      metadata = provider.setup_provider_metadata(provider_name, config)
      
      expect(metadata).to eq({ source_name: provider_name })
      expect(Utils::ProviderConfig).to have_received(:build_metadata).with(
        hash_including(source_name: provider_name, base_currency: "USD")
      )
    end

    it "uses provided source display name if available" do
      config = {
        source_display_name: "Custom Name",
        base_currency: "USD"
      }
      
      allow(Utils::ProviderConfig).to receive(:build_metadata).and_return({ source_name: "Custom Name" })
      
      metadata = provider.setup_provider_metadata(provider_name, config)
      
      expect(Utils::ProviderConfig).to have_received(:build_metadata).with(
        hash_including(source_name: "Custom Name")
      )
    end

    it "sets publication_time if all required values are present" do
      config = {
        base_currency: "USD",
        publication_hour: 14,
        publication_minute: 30,
        publication_timezone: "+01:00"
      }
      
      publication_time = Time.new(2025, 1, 1, 14, 30, 0, "+01:00")
      allow(provider).to receive(:format_publication_time).and_return(publication_time)
      allow(Utils::ProviderConfig).to receive(:build_metadata).and_return({})
      
      provider.setup_provider_metadata(provider_name, config)
      
      expect(provider).to have_received(:format_publication_time).with(14, 30, "+01:00")
      expect(Utils::ProviderConfig).to have_received(:build_metadata).with(
        hash_including(publication_time: publication_time)
      )
    end
  end

  describe "#standard_metadata" do
    it "returns a hash with standard metadata" do
      base_currency = "USD"
      publication_time = Time.new(2025, 1, 1, 14, 30, 0)
      supported_currencies = ["USD", "EUR", "GBP"]
      
      metadata = provider.standard_metadata(
        base_currency: base_currency,
        publication_time: publication_time,
        working_days_only: true,
        supported_currencies: supported_currencies
      )
      
      expect(metadata).to include(
        update_frequency: :daily,
        publication_time: publication_time,
        supports_historical: true,
        base_currency: base_currency,
        working_days_only: true,
        supported_currencies: supported_currencies
      )
    end
    
    it "allows working_days_only to be customized" do
      metadata = provider.standard_metadata(
        base_currency: "USD",
        publication_time: Time.now,
        working_days_only: false
      )
      
      expect(metadata[:working_days_only]).to be false
    end
  end

  describe "#format_publication_time" do
    it "formats time from components" do
      hour = 14
      minute = 30
      timezone = "+01:00"
      
      time = provider.format_publication_time(hour, minute, timezone)
      
      expect(time.hour).to eq(14)
      expect(time.min).to eq(30)
    end
    
    it "returns nil for invalid inputs" do
      expect(provider.format_publication_time(nil, nil, nil)).to be_nil
    end
  end
end 