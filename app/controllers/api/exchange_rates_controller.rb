# frozen_string_literal: true

require_relative 'base_controller'
require_relative '../../services/exchange_rate_service'
require_relative '../../errors/unsupported_provider_error'

module Api
  class ExchangeRatesController < BaseController
    DEFAULT_PROVIDER = 'CZK'

    api_route :get, '/exchange_rates' do
      source = params['source'] || DEFAULT_PROVIDER
      currencies = params['currencies']&.split(',')&.map { |c| c.strip.upcase } || []

      begin
        rates = ExchangeRateService.get_rates(source, currencies)
        status 200
        { rates: rates }.to_json
      rescue UnsupportedProviderError => e
        status 422
        { error: e.message }.to_json
      rescue ProviderNotAvailableError => e
        status 503
        { error: "Service unavailable: #{e.message}" }.to_json
      end
    end
  end
end
