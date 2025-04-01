require 'net/http'
require_relative '../errors/exchange_rate_errors'
require_relative 'http_fetcher_constants'

module Fetchers
  # Handles making HTTP requests
  class HttpClient
    def initialize(options = {})
      @options = options
      @options[:read_timeout] ||= HttpFetcherConstants::DEFAULT_TIMEOUT
      @options[:open_timeout] ||= HttpFetcherConstants::DEFAULT_TIMEOUT
    end

    def fetch_response(uri)
      if uri.scheme == 'https'
        fetch_https(uri)
      else
        fetch_http(uri)
      end
    end

    private

    def fetch_http(uri)
      fetch_with_protocol(uri, false)
    end

    def fetch_https(uri)
      fetch_with_protocol(uri, true)
    end

    def fetch_with_protocol(uri, use_ssl = false)
      http_args = [uri.host, uri.port]
      http_args << { use_ssl: true } if use_ssl

      Net::HTTP.start(*http_args) do |http|
        http.read_timeout = @options[:read_timeout] if @options[:read_timeout]
        http.open_timeout = @options[:open_timeout] if @options[:open_timeout]

        http.verify_mode = OpenSSL::SSL::VERIFY_PEER if use_ssl

        request = Net::HTTP::Get.new(uri.request_uri)
        add_headers(request, @options[:headers])

        response = http.request(request)
        
        if response.is_a?(Net::HTTPRedirection)
          handle_redirect(response, uri)
        else
          response
        end
      end
    end

    def add_headers(request, headers)
      return unless headers.is_a?(Hash)

      headers.each do |key, value|
        request[key] = value
      end
    end

    def handle_redirect(response, uri)
      redirect_count = @options[:redirects].to_i

      if redirect_count >= 5
        raise ExchangeRateErrors::NetworkError.new("Too many redirects")
      end

      location = response['location']
      new_uri = URI.parse(location.start_with?('/') ? "#{uri.scheme}://#{uri.host}#{location}" : location)
      
      @options[:redirects] = redirect_count + 1
      fetch_response(new_uri)
    end
  end
end 