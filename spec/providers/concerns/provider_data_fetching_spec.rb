require 'rails_helper'
require_relative 'shared_examples'

RSpec.describe ProviderDataFetching do
  include_examples "a provider concern"

  let(:provider) { TestProvider.new }
  let(:provider_name) { "TestProvider" }
  let(:base_currency) { "USD" }

  describe "#parse_data_with_adapter" do
    let(:response) { { data: "test data" } }
    let(:content_type) { "application/json" }
    let(:file_extension) { ".json" }
    let(:rates) { [double("ExchangeRate")] }
    let(:adapter) { double("Adapter", parse: rates) }

    context "with content type available" do
      it "uses the content type to select adapter" do
        allow(AdapterFactory).to receive(:for_content_type).and_return(adapter)
        
        result = provider.parse_data_with_adapter(response, content_type, file_extension, provider_name, base_currency)
        
        expect(result).to eq(rates)
        expect(AdapterFactory).to have_received(:for_content_type).with(provider_name, content_type)
        expect(adapter).to have_received(:parse).with("test data", base_currency)
      end
    end

    context "with file extension available" do
      let(:response) { { data: "test data" } }
      
      it "uses the file extension to select adapter when content type is not available" do
        allow(AdapterFactory).to receive(:for_file_extension).and_return(adapter)
        
        result = provider.parse_data_with_adapter(response, nil, file_extension, provider_name, base_currency)
        
        expect(result).to eq(rates)
        expect(AdapterFactory).to have_received(:for_file_extension).with(provider_name, file_extension)
        expect(adapter).to have_received(:parse).with("test data", base_currency)
      end
    end

    context "with neither content type nor file extension available" do
      let(:response) { { data: "test data" } }
      
      it "auto-detects adapter from content" do
        allow(AdapterFactory).to receive(:for_content).and_return(adapter)
        
        result = provider.parse_data_with_adapter(response, nil, nil, provider_name, base_currency)
        
        expect(result).to eq(rates)
        expect(AdapterFactory).to have_received(:for_content).with(provider_name, "test data")
        expect(adapter).to have_received(:parse).with("test data", base_currency)
      end
    end

    context "with content type in response" do
      let(:response) { { data: "test data", content_type: "text/xml" } }
      
      it "prefers content type from response" do
        allow(AdapterFactory).to receive(:for_content_type).and_return(adapter)
        
        result = provider.parse_data_with_adapter(response, content_type, file_extension, provider_name, base_currency)
        
        expect(result).to eq(rates)
        expect(AdapterFactory).to have_received(:for_content_type).with(provider_name, "text/xml")
        expect(adapter).to have_received(:parse).with("test data", base_currency)
      end
    end
  end

  describe "#fetch_http_data" do
    let(:url) { "https://api.example.com/rates" }
    let(:headers) { { "Accept" => "application/json" } }
    let(:retries) { 3 }
    let(:response) { { data: "test data", content_type: "application/json" } }

    it "calls HttpFetcher.fetch with the correct parameters" do
      allow(HttpFetcher).to receive(:fetch).and_return(response)
      
      result = provider.fetch_http_data(url, headers, retries, provider_name)
      
      expect(result).to eq(response)
      expect(HttpFetcher).to have_received(:fetch).with(url, headers, retries, provider_name)
    end

    it "provides default values for headers and retries" do
      # Bypass BaseProvider's method and call the concern's method directly
      module TestDataFetching
        include ProviderDataFetching
      end
      test_object = Object.new.extend(TestDataFetching)
      
      allow(HttpFetcher).to receive(:fetch).and_return(response)
      
      result = test_object.fetch_http_data(url, {}, 3, nil)
      
      expect(result).to eq(response)
      expect(HttpFetcher).to have_received(:fetch).with(url, {}, 3, nil)
    end
  end
end 