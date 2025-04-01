require 'open-uri'
require 'net/http'

module Fetchers
  # Handles retry logic
  class RetryHandler
    def initialize(max_retries, initial_delay)
      @max_retries = max_retries
      @initial_delay = initial_delay
    end

    def with_retry
      retries = 0
      delay = @initial_delay

      begin
        yield
      rescue OpenURI::HTTPError, SocketError, Timeout::Error, Net::OpenTimeout, Net::ReadTimeout => e
        if should_retry?(e, retries)
          retries += 1
          sleep(delay)
          delay *= 2 # Exponential backoff
          retry
        else
          raise
        end
      end
    end

    private

    def should_retry?(error, retry_count)
      return false if retry_count >= @max_retries
      
      # HTTP errors need to be checked for status code
      if error.is_a?(OpenURI::HTTPError)
        status = error.io.status.first.to_i
        return retryable_status?(status)
      end
      
      # Connection and timeout errors are always retryable
      error.is_a?(SocketError) || 
        error.is_a?(Timeout::Error) || 
        error.is_a?(Net::OpenTimeout) || 
        error.is_a?(Net::ReadTimeout)
    end

    def retryable_status?(status)
      # 408 Request Timeout, 429 Too Many Requests, 5xx Server errors
      status == 408 || status == 429 || (status >= 500 && status < 600)
    end
  end
end 