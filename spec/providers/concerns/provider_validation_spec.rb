require 'rails_helper'
require_relative 'shared_examples'

RSpec.describe ProviderValidation do
  include_examples "a provider concern"

  let(:provider) { TestProvider.new }
  let(:base_currency) { "USD" }
  let(:provider_name) { "TestProvider" }

  describe "#validate_rates" do
    context "with valid rates" do
      let(:rates) do
        [
          instance_double(ExchangeRate, from: double(code: base_currency), to: double(code: "EUR")),
          instance_double(ExchangeRate, from: double(code: base_currency), to: double(code: "GBP"))
        ]
      end

      it "does not raise an error" do
        expect { provider.validate_rates(rates, base_currency, provider_name) }.not_to raise_error
      end

      it "calls validate_provider_specific_rates if defined" do
        allow(provider).to receive(:validate_provider_specific_rates)
        provider.validate_rates(rates, base_currency, provider_name)
        expect(provider).to have_received(:validate_provider_specific_rates).with(rates)
      end
    end

    context "with empty rates" do
      it "raises ValidationError" do
        expect do
          provider.validate_rates([], base_currency, provider_name)
        end.to raise_error(ExchangeRateErrors::ValidationError, /No exchange rates found/)
      end
    end

    context "with incorrect base currency" do
      let(:rates) do
        [
          instance_double(ExchangeRate, from: double(code: "EUR"), to: double(code: "USD")),
          instance_double(ExchangeRate, from: double(code: base_currency), to: double(code: "GBP"))
        ]
      end

      it "raises ValidationError" do
        expect do
          provider.validate_rates(rates, base_currency, provider_name)
        end.to raise_error(ExchangeRateErrors::ValidationError, /Unexpected base currency/)
      end
    end
  end

  describe "#raise_validation_error" do
    # Create a simple test implementation directly including the concern to avoid method visibility issues
    module TestValidation
      include ProviderValidation

      def test_raise_error(message, provider_name, context = {})
        raise_validation_error(message, provider_name, context)
      end
    end

    let(:test_validator) { Object.new.extend(TestValidation) }

    it "raises ValidationError with the correct parameters" do
      message = "Test validation error"
      context = { test: "value" }

      allow(ExchangeRateErrors::ValidationError).to receive(:new).and_call_original

      expect do
        test_validator.test_raise_error(message, provider_name, context)
      end.to raise_error(ExchangeRateErrors::ValidationError, message)

      expect(ExchangeRateErrors::ValidationError).to have_received(:new).with(
        message, nil, provider_name, context
      )
    end
  end
end