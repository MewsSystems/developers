require 'open-uri'
require 'net/http'
require_relative '../errors/exchange_rate_errors'

class HttpFetcher
  MAX_RETRIES = 3
  RETRY_DELAY = 1 # seconds
  DEFAULT_TIMEOUT = 10 # seconds
  
  # Fetch data from a URL with retry logic
  # @param url [String] URL to fetch data from
  # @param options [Hash] Additional options for the request
  # @param max_retries [Integer] Maximum number of retries
  # @param provider [String] Provider name for error reporting
  # @return [Hash] Hash with :data and :content_type
  def self.fetch(url, options = {}, max_retries = MAX_RETRIES, provider = nil)
    options[:read_timeout] ||= DEFAULT_TIMEOUT
    options[:open_timeout] ||= DEFAULT_TIMEOUT
    
    retries = 0
    delay = RETRY_DELAY
    
    begin
      uri = URI.parse(url)
      
      # Use Net::HTTP instead of open-uri for more control
      response = if uri.scheme == 'https'
                   fetch_https(uri, options)
                 else
                   fetch_http(uri, options)
                 end
      
      # Get content type from response
      content_type = response['Content-Type']
      
      # Return the response body and content type
      {
        data: response.body,
        content_type: content_type,
        status: response.code.to_i,
        headers: response.to_hash,
        last_modified: response['Last-Modified']
      }
      
    rescue OpenURI::HTTPError => e
      # Convert HTTP error status to appropriate exception
      status = e.io.status.first.to_i
      message = "HTTP error: #{e.message} (#{e.io.status.join(' ')})"
      error = ExchangeRateErrors.from_http_status(status, message, provider)
      
      if retryable_error?(status) && retries < max_retries
        retries += 1
        sleep(delay)
        delay *= 2 # Exponential backoff
        retry
      end
      
      raise error
      
    rescue SocketError => e
      error = ExchangeRateErrors::ConnectionError.new(
        "Connection error: #{e.message}", e, provider
      )
      
      if retries < max_retries
        retries += 1
        sleep(delay)
        delay *= 2 # Exponential backoff
        retry
      end
      
      raise error
      
    rescue Timeout::Error, Net::OpenTimeout, Net::ReadTimeout => e
      error = ExchangeRateErrors::TimeoutError.new(
        "Timeout error: #{e.message}", e, provider
      )
      
      if retries < max_retries
        retries += 1
        sleep(delay)
        delay *= 2 # Exponential backoff
        retry
      end
      
      raise error
      
    rescue OpenSSL::SSL::SSLError => e
      raise ExchangeRateErrors::SSLError.new(
        "SSL error: #{e.message}", e, provider
      )
      
    rescue => e
      # Catch all other errors
      raise ExchangeRateErrors::NetworkError.new(
        "Unexpected error while fetching data: #{e.message}", e, provider
      )
    end
  end
  
  private
  
  # Determine if an error is retryable based on HTTP status code
  # @param status [Integer] HTTP status code
  # @return [Boolean] Whether the error is retryable
  def self.retryable_error?(status)
    # 408 Request Timeout, 429 Too Many Requests, 5xx Server errors
    status == 408 || status == 429 || (status >= 500 && status < 600)
  end
  
  # Fetch HTTP URL
  # @param uri [URI] URI to fetch
  # @param options [Hash] Request options
  # @return [Net::HTTPResponse] HTTP response
  def self.fetch_http(uri, options = {})
    fetch_with_protocol(uri, options, false)
  end
  
  # Fetch HTTPS URL
  # @param uri [URI] URI to fetch
  # @param options [Hash] Request options
  # @return [Net::HTTPResponse] HTTP response
  def self.fetch_https(uri, options = {})
    fetch_with_protocol(uri, options, true)
  end
  
  # Common method for fetching via HTTP or HTTPS
  # @param uri [URI] URI to fetch
  # @param options [Hash] Request options
  # @param use_ssl [Boolean] Whether to use SSL
  # @return [Net::HTTPResponse] HTTP response
  def self.fetch_with_protocol(uri, options = {}, use_ssl = false)
    http_args = [uri.host, uri.port]
    http_args << { use_ssl: true } if use_ssl
    
    Net::HTTP.start(*http_args) do |http|
      http.read_timeout = options[:read_timeout] if options[:read_timeout]
      http.open_timeout = options[:open_timeout] if options[:open_timeout]
      
      # Set SSL options if using HTTPS
      http.verify_mode = OpenSSL::SSL::VERIFY_PEER if use_ssl
      
      request = Net::HTTP::Get.new(uri.request_uri)
      add_headers(request, options[:headers])
      
      response = http.request(request)
      
      # Handle redirects
      handle_redirect(response, uri, options)
    end
  end
  
  # Add headers to request
  # @param request [Net::HTTP::Request] Request object
  # @param headers [Hash] Headers to add
  def self.add_headers(request, headers)
    return unless headers.is_a?(Hash)
    
    headers.each do |key, value|
      request[key] = value
    end
  end
  
  # Handle HTTP redirects
  # @param response [Net::HTTPResponse] HTTP response
  # @param uri [URI] Original URI
  # @param options [Hash] Request options
  # @return [Net::HTTPResponse] Final HTTP response
  def self.handle_redirect(response, uri, options)
    case response
    when Net::HTTPRedirection
      # Limit to 5 redirects to prevent infinite loops
      if options[:redirects].to_i >= 5
        raise ExchangeRateErrors::NetworkError.new(
          "Too many redirects"
        )
      end
      
      # Get the redirect URL
      location = response['location']
      new_uri = URI.parse(location.start_with?('/') ? "#{uri.scheme}://#{uri.host}#{location}" : location)
      
      # Follow the redirect
      options[:redirects] = options[:redirects].to_i + 1
      
      if new_uri.scheme == 'https'
        fetch_https(new_uri, options)
      else
        fetch_http(new_uri, options)
      end
    else
      response
    end
  end
end 