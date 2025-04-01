module Api
  module V1
    class ExchangeRatesController < ApplicationController
      include ExchangeRateErrorHandler
      include ExchangeRateResponseFormatter

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

      # Get the exchange rate service
      def exchange_rate_service
        @exchange_rate_service ||= ExchangeRateApplication.exchange_rate_service
      end
    end
  end
end