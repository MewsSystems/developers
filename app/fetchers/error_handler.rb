require 'open-uri'
require 'openssl'
require_relative '../errors/exchange_rate_errors'

module Fetchers
  # Handles errors during the HTTP fetch process
  class ErrorHandler
    def initialize(provider = nil)
      @provider = provider
    end

    def handle_error(error)
      case error
      when OpenURI::HTTPError
        status = error.io.status.first.to_i
        message = "HTTP error: #{error.message} (#{error.io.status.join(' ')})"
        raise ExchangeRateErrors.from_http_status(status, message, @provider)
      when SocketError
        raise ExchangeRateErrors::ConnectionError.new(
          "Connection error: #{error.message}", error, @provider
        )
      when Timeout::Error, Net::OpenTimeout, Net::ReadTimeout
        raise ExchangeRateErrors::TimeoutError.new(
          "Timeout error: #{error.message}", error, @provider
        )
      when OpenSSL::SSL::SSLError
        raise ExchangeRateErrors::SSLError.new(
          "SSL error: #{error.message}", error, @provider
        )
      else
        # Catch all other errors
        raise ExchangeRateErrors::NetworkError.new(
          "Unexpected error while fetching data: #{error.message}", error, @provider
        )
      end
    end
  end
end