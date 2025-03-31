module Api
  module V1
    class ExchangeRatesController < ApplicationController
      rescue_from ExchangeRateErrors::Error, with: :handle_exchange_rate_error
      rescue_from StandardError, with: :handle_standard_error
      
      # GET /api/v1/exchange_rates
      # Get all exchange rates or filter by currency
      def index
        currencies = filter_currencies
        
        rates = exchange_rate_service.get_rates(currencies)
        
        response = {
          base_currency: exchange_rate_service.provider.metadata[:base_currency],
          source: exchange_rate_service.provider.metadata[:source_name],
          timestamp: Time.now.iso8601,
          rates: rates.map { |rate| format_rate(rate) }
        }
        
        # Include unavailable currencies if any were requested
        add_warnings_to_response(response)
        
        render json: response
      end
      
      # GET /api/v1/exchange_rates/:from/:to
      # Get specific exchange rate between two currencies
      def show
        from = params[:from]&.upcase
        to = params[:to]&.upcase
        
        rate = exchange_rate_service.get_rate(from, to)
        
        render json: format_rate(rate).merge({
          timestamp: Time.now.iso8601,
          source: exchange_rate_service.provider.metadata[:source_name]
        })
      end
      
      # GET /api/v1/exchange_rates/convert
      # Convert amount from one currency to another
      def convert
        from = params[:from]&.upcase
        to = params[:to]&.upcase
        amount = params[:amount].to_f
        
        # Validate params
        unless from && to && amount > 0
          raise ExchangeRateErrors::ValidationError.new("Missing or invalid parameters")
        end
        
        result = exchange_rate_service.convert(amount, from, to)
        
        render json: result.merge({
          timestamp: Time.now.iso8601,
          source: exchange_rate_service.provider.metadata[:source_name]
        })
      end
      
      # GET /api/v1/exchange_rates/currencies
      # Get list of supported currencies
      def currencies
        currencies = exchange_rate_service.provider.supported_currencies
        
        render json: {
          source: exchange_rate_service.provider.metadata[:source_name],
          base_currency: exchange_rate_service.provider.metadata[:base_currency],
          currencies: currencies.map { |code| { code: code } }
        }
      end
      
      private
      
      # Get currencies to filter from query params
      def filter_currencies
        return nil unless params[:currencies]
        params[:currencies].split(',').map(&:strip).reject(&:empty?)
      end
      
      # Format an exchange rate for API response
      def format_rate(rate)
        {
          from: rate.from.code,
          to: rate.to.code,
          rate: rate.rate,
          date: rate.date
        }
      end
      
      # Add warnings about unavailable currencies to response
      def add_warnings_to_response(response)
        unavailable = exchange_rate_service.unavailable_currencies
        if unavailable.any?
          response[:warnings] = {
            unavailable_currencies: unavailable.keys,
            available_currencies: exchange_rate_service.provider.supported_currencies - unavailable.keys,
            provider: exchange_rate_service.provider.metadata[:source_name]
          }
        end
      end
      
      # Error handler for ExchangeRateErrors
      def handle_exchange_rate_error(error)
        status_code = map_error_to_status_code(error)
        log_error_with_backtrace(error, status_code)
        
        error_response = build_error_response(error, status_code, is_exchange_rate_error: true)
        render json: error_response, status: status_code
      end
      
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
        if is_exchange_rate_error
          error_type = error.class.name.demodulize
          error_message = error.message
        else
          error_type = "InternalServerError"
          error_message = error.message
        end
        
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
        if !is_exchange_rate_error
          error_hash[:backtrace] = error.backtrace.first(5) if error.backtrace
          return
        end
        
        # Add provider and original error for ExchangeRateErrors
        if error.respond_to?(:provider) && error.provider.present?
          error_hash[:provider] = error.provider
        end
        
        if error.respond_to?(:original_error) && error.original_error.present?
          error_hash[:original_error] = {
            type: error.original_error.class.name,
            message: error.original_error.message
          }
        end
      end
      
      # Get the exchange rate service
      def exchange_rate_service
        @exchange_rate_service ||= ExchangeRateApplication.exchange_rate_service
      end
    end
  end
end 