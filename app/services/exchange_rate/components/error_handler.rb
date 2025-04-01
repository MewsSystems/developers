require_relative '../../../errors/exchange_rate_errors'

# Class responsible for handling exchange rate errors
class ExchangeRateErrorHandler
  # Handle errors in rate service operations
  # @param e [Exception] The exception to handle
  # @param provider_name [String] The name of the provider
  # @raise [ExchangeRateErrors::Error] A wrapped exchange rate error
  def self.handle_error(e, provider_name)
    if e.is_a?(ExchangeRateErrors::Error)
      # Re-raise ExchangeRateErrors
      raise e
    else
      # Wrap unexpected errors
      raise ExchangeRateErrors::ServiceError.new(
        "Unexpected error in exchange rate service: #{e.message}",
        e, provider_name
      )
    end
  end
end 