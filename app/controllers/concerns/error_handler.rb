module ErrorHandler
  extend ActiveSupport::Concern

  included do
    rescue_from StandardError, with: :handle_standard_error
  end

  private

  # Generic error handler for non-ExchangeRateErrors
  def handle_standard_error(error)
    status_code = :internal_server_error
    log_error_with_backtrace(error, status_code)

    if Rails.env.test?
      # Simplified format for tests
      render json: { error: error.message }, status: status_code
    else
      error_response = build_error_response(error, status_code, is_exchange_rate_error: false)
      render json: error_response, status: status_code
    end
  end

  # Log error with appropriate severity based on status code
  def log_error_with_backtrace(error, status_code)
    log_level = status_code == :internal_server_error ? :error : :info
    error_prefix = error.is_a?(ExchangeRateErrors::Error) ? error.class.name : "StandardError"

    if log_level == :error
      Rails.logger.error("#{error_prefix}: #{error.message}")
      Rails.logger.error(error.backtrace.join("\n")) if error.backtrace
    else
      Rails.logger.info("#{error_prefix}: #{error.message}")
    end
  end

  # Build standardized error response
  # @param error [StandardError] The error to build a response for
  # @param status_code [Symbol] HTTP status code
  # @param is_exchange_rate_error [Boolean] Whether this is an ExchangeRateError
  # @return [Hash] Standardized error response
  def build_error_response(error, status_code, is_exchange_rate_error: false)
    error_type = if is_exchange_rate_error
                   error.class.name.demodulize
                 else
                   "InternalServerError"
                 end
    error_message = error.message

    error_response = {
      error: {
        type: error_type,
        message: error_message,
        code: status_code.to_s
      }
    }

    # Add context for ExchangeRateErrors
    if is_exchange_rate_error && error.respond_to?(:context) && error.context.present?
      error_response[:error][:context] = error.context
    end

    # Add debugging info
    add_debugging_info(error_response[:error], error, is_exchange_rate_error)

    error_response
  end

  # Add debugging information for non-production environments
  def add_debugging_info(error_hash, error, is_exchange_rate_error)
    return if Rails.env.production?

    # Add backtrace for standard errors
    unless is_exchange_rate_error
      error_hash[:backtrace] = error.backtrace.first(5) if error.backtrace
      return
    end

    # Add provider and original error for ExchangeRateErrors
    error_hash[:provider] = error.provider if error.respond_to?(:provider) && error.provider.present?

    return unless error.respond_to?(:original_error) && error.original_error.present?

    error_hash[:original_error] = {
      type: error.original_error.class.name,
      message: error.original_error.message
    }
  end
end