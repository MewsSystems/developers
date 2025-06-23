module ExchangeRateAdapterInterface
  # Parse raw data into exchange rate domain objects
  # @param data [String] Raw data from the provider
  # @param base_currency [String] Base currency code
  # @return [Array<ExchangeRate>] Array of exchange rate objects
  def parse(data, base_currency)
    raise NotImplementedError, "#{self.class.name} must implement parse"
  end

  # Check if this adapter can handle the given content type
  # @param content_type [String] Content type to check
  # @return [Boolean] Whether this adapter can handle the content type
  def supports_content_type?(content_type)
    raise NotImplementedError, "#{self.class.name} must implement supports_content_type?"
  end

  # Check if this adapter can handle the given content
  # @param content [String] Content to check
  # @return [Boolean] Whether this adapter can handle the content
  def supports_content?(content)
    raise NotImplementedError, "#{self.class.name} must implement supports_content?"
  end

  # Get the name of this adapter
  # @return [String] Adapter name
  def name
    self.class.name.gsub('Adapter', '')
  end
end