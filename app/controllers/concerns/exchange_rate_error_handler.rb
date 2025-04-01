module ExchangeRateErrorHandler
  extend ActiveSupport::Concern
  include ErrorHandler

  included do
    rescue_from ExchangeRateErrors::Error, with: :handle_exchange_rate_error
  end

  private

  # Error handler for ExchangeRateErrors
  def handle_exchange_rate_error(error)
    status_code = map_error_to_status_code(error)
    log_error_with_backtrace(error, status_code)

    error_response = build_error_response(error, status_code, is_exchange_rate_error: true)
    render json: error_response, status: status_code
  end

  # Map error type to HTTP status code
  def map_error_to_status_code(error)
    case error
    when ExchangeRateErrors::ValidationError, ExchangeRateErrors::CurrencyNotSupportedError
      :bad_request # 400
    when ExchangeRateErrors::ProviderAuthenticationError
      :unauthorized # 401
    when ExchangeRateErrors::ProviderRateLimitError
      :too_many_requests # 429
    when ExchangeRateErrors::ProviderUnavailableError, ExchangeRateErrors::ProviderMaintenanceError
      :service_unavailable # 503
    when ExchangeRateErrors::TimeoutError
      :gateway_timeout # 504
    when ExchangeRateErrors::InvalidConfigurationError, ExchangeRateErrors::StorageError
      :internal_server_error # 500
    else
      :internal_server_error # 500
    end
  end
end 