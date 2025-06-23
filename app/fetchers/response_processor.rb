module Fetchers
  # Processes HTTP responses
  class ResponseProcessor
    def self.process(response)
      {
        data: response.body,
        content_type: response['Content-Type'],
        status: response.code.to_i,
        headers: response.to_hash,
        last_modified: response['Last-Modified']
      }
    end
  end
end