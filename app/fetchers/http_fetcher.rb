require 'uri'
require_relative 'http_client'
require_relative 'retry_handler'
require_relative 'response_processor'
require_relative 'error_handler'
require_relative 'http_fetcher_constants'

module Fetchers
  # The main class that orchestrates HTTP fetching
  class HttpFetcher
    # Fetch data from a URL with retry logic
    # @param url [String] URL to fetch data from
    # @param headers [Hash] HTTP headers for the request
    # @param max_retries [Integer] Maximum number of retries
    # @param provider [String] Provider name for error reporting
    # @return [Hash] Hash with :data and :content_type
    def self.fetch(url, headers = {}, max_retries = HttpFetcherConstants::MAX_RETRIES, provider = nil)
      options = { headers: headers }
      client = HttpClient.new(options)
      retry_handler = RetryHandler.new(max_retries, HttpFetcherConstants::RETRY_DELAY)
      error_handler = ErrorHandler.new(provider)

      retry_handler.with_retry do
        uri = URI.parse(url)
        response = client.fetch_response(uri)
        ResponseProcessor.process(response)
      rescue => e
        error_handler.handle_error(e)
      end
    end
  end
end

# Create an alias for backward compatibility
HttpFetcher = Fetchers::HttpFetcher