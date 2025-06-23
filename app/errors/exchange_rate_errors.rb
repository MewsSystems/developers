module ExchangeRateErrors
  # Base error class for all exchange rate errors
  class Error < StandardError
    attr_reader :original_error, :provider, :metadata

    def initialize(message, original_error = nil, provider = nil, metadata = {})
      super(message)
      @original_error = original_error
      @provider = provider
      @metadata = metadata || {}
    end

    # Convert to a structured hash for API responses
    def to_hash
      {
        error: self.class.name.demodulize,
        message: message,
        provider: provider,
        context: metadata
      }
    end
  end

  # HTTP/Network related errors
  class NetworkError < Error; end
  class ConnectionError < NetworkError; end
  class TimeoutError < NetworkError; end
  class SSLError < NetworkError; end

  # Provider errors
  class ProviderError < Error; end
  class ProviderUnavailableError < ProviderError; end
  class ProviderRateLimitError < ProviderError; end
  class ProviderAuthenticationError < ProviderError; end
  class ProviderMaintenanceError < ProviderError; end

  # Data format errors
  class DataError < Error; end
  class ParseError < DataError; end
  class ValidationError < DataError; end
  class UnsupportedFormatError < DataError; end

  # Service errors
  class ServiceError < Error; end
  class CurrencyNotSupportedError < ServiceError; end
  class NoExchangeRateAvailableError < ServiceError; end
  class InvalidConfigurationError < ServiceError; end
  class CacheError < ServiceError; end

  # Repository errors
  class RepositoryError < Error; end
  class StorageError < RepositoryError; end
  class StaleDataError < RepositoryError; end

  # Helper method to wrap errors with context
  def self.wrap_error(error, error_class = nil, message = nil, provider = nil, context = {})
    error_class ||= error.is_a?(StandardError) ? Error : error.class

    if error.is_a?(Error)
      # If already an ExchangeRateErrors::Error, just return it
      error
    else
      # Wrap the error
      error_class.new(message, error, provider, context)
    end
  end

  # Map HTTP status codes to appropriate error classes
  def self.from_http_status(status, message = nil, provider = nil)
    error_class = case status
                  when 400..499
                    case status
                    when 401, 403
                      ProviderAuthenticationError
                    when 404
                      ProviderUnavailableError
                    when 408
                      TimeoutError
                    when 429
                      ProviderRateLimitError
                    else
                      ProviderError
                    end
                  when 500..599
                    case status
                    when 503, 504
                      ProviderMaintenanceError
                    else
                      ProviderUnavailableError
                    end
                  else
                    Error
                  end

    error_class.new(message, nil, provider)
  end
end