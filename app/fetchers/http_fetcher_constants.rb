module Fetchers
  # Constants used by HTTP fetching classes
  module HttpFetcherConstants
    MAX_RETRIES = 3
    RETRY_DELAY = 1 # seconds
    DEFAULT_TIMEOUT = 10 # seconds
  end
end