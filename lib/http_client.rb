# frozen_string_literal: true

require 'faraday'
require_relative '../app/errors/provider_not_available_error'

class HTTPClient
  MAX_RETRIES = 2

  def self.get(url, source_provider)
    attempts = 0

    begin
      response = Faraday.get(url)
    rescue Faraday::Error
      attempts += 1
      retry if attempts <= MAX_RETRIES
      raise ProviderNotAvailableError, source_provider
    end

    response.body
  end
end
