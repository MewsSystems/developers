module ProviderDataFetching
  extend ActiveSupport::Concern

  # Parse data into exchange rates using appropriate adapter
  # @param response [Hash] Response data with content type and raw data
  # @param content_type [String] Content type to use if not in response
  # @param file_extension [String] File extension to use for adapter selection
  # @param provider_name [String] Provider name for error messages
  # @param base_currency [String] Base currency for rates
  # @return [Array<ExchangeRate>] Parsed exchange rates
  def parse_data_with_adapter(response, content_type, file_extension, provider_name, base_currency)
    # Figure out the right adapter to use
    content_type_to_use = response[:content_type] || content_type
    file_extension_to_use = response[:file_extension] || file_extension

    adapter = if content_type_to_use
                AdapterFactory.for_content_type(provider_name, content_type_to_use)
              elsif file_extension_to_use
                AdapterFactory.for_file_extension(provider_name, file_extension_to_use)
              else
                # Try to auto-detect from content
                AdapterFactory.for_content(provider_name, response[:data])
              end

    # Parse the data
    adapter.parse(response[:data], base_currency)
  end

  # Fetch data from a URL using HTTP
  # @param url [String] URL to fetch data from
  # @param provider_name [String] Provider name for error messages
  # @param headers [Hash] HTTP headers to send
  # @param retries [Integer] Number of retries
  # @return [Hash] Response with data and content type
  def fetch_http_data(url, provider_name, headers = {}, retries = 3)
    HttpFetcher.fetch(url, headers, retries, provider_name)
  end
end