require 'rails_helper'
require_relative 'shared_examples'

RSpec.describe ProviderOperations do
  include_examples "a provider concern"

  let(:provider) { TestProvider.new }
  let(:provider_name) { "TestProvider" }

  describe "#perform_provider_operation" do
    it "executes the block and returns its result" do
      result = provider.perform_provider_operation(provider_name, "test operation") { 42 }
      expect(result).to eq(42)
    end

    it "calls Utils::ProviderHelper.with_provider_error_handling" do
      allow(Utils::ProviderHelper).to receive(:with_provider_error_handling).and_yield
      
      provider.perform_provider_operation(provider_name, "test operation") { 42 }
      
      expect(Utils::ProviderHelper).to have_received(:with_provider_error_handling).with(
        provider_name, "test operation"
      )
    end

    it "propagates errors from the helper" do
      error = RuntimeError.new("Test error")
      allow(Utils::ProviderHelper).to receive(:with_provider_error_handling).and_raise(error)
      
      expect {
        provider.perform_provider_operation(provider_name, "test operation") { 42 }
      }.to raise_error(RuntimeError, "Test error")
    end
  end

  describe "#extract_supported_currencies" do
    it "calls Utils::CurrencyHelper.extract_currency_codes" do
      rates = [double, double]
      allow(Utils::CurrencyHelper).to receive(:extract_currency_codes).and_return(["USD", "EUR"])
      
      result = provider.extract_supported_currencies(rates)
      
      expect(result).to eq(["USD", "EUR"])
      expect(Utils::CurrencyHelper).to have_received(:extract_currency_codes).with(rates)
    end
  end

  describe "#supports_currency?" do
    context "when called with one argument" do
      before do
        allow(provider).to receive(:supported_currencies).and_return(["USD", "EUR", "GBP"])
        provider.instance_variable_set(:@supported_currencies, ["USD", "EUR", "GBP"])
      end

      it "returns true for supported currencies" do
        expect(provider.supports_currency?("USD")).to be true
        expect(provider.supports_currency?("EUR")).to be true
      end

      it "returns false for unsupported currencies" do
        expect(provider.supports_currency?("XYZ")).to be false
      end

      it "handles case insensitivity" do
        expect(provider.supports_currency?("usd")).to be true
        expect(provider.supports_currency?("eUr")).to be true
      end

      it "handles nil and empty values" do
        expect(provider.supports_currency?(nil)).to be false
        expect(provider.supports_currency?("")).to be false
      end
    end

    context "when called with two arguments" do
      let(:supported_currencies) { ["USD", "EUR", "GBP"] }

      it "returns true for supported currencies" do
        expect(provider.supports_currency?(supported_currencies, "USD")).to be true
        expect(provider.supports_currency?(supported_currencies, "EUR")).to be true
      end

      it "returns false for unsupported currencies" do
        expect(provider.supports_currency?(supported_currencies, "XYZ")).to be false
      end

      it "handles case insensitivity" do
        expect(provider.supports_currency?(supported_currencies, "usd")).to be true
        expect(provider.supports_currency?(supported_currencies, "eUr")).to be true
      end

      it "handles nil and empty values" do
        expect(provider.supports_currency?(supported_currencies, nil)).to be false
        expect(provider.supports_currency?(supported_currencies, "")).to be false
      end
    end
  end
end 