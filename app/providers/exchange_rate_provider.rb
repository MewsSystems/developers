# frozen_string_literal: true

require 'faraday'

require_relative '../../lib/http_client'

class ExchangeRateProvider
  MAX_RETRIES = 2

  def request_exchange_rates
    response_body = HTTPClient.get(api_url, source_provider)
    parse_response(response_body)
  end

  private

  def api_url
    raise NotImplementedError, 'Subclasses must define api_url'
  end

  def source_provider
    raise NotImplementedError, 'Subclasses must define source_provider'
  end

  def parse_response
    raise NotImplementedError, 'Subclasses must define parse_response'
  end
end
