module Adapters
  module Strategies
    class EcbXmlAdapter
      def initialize(source_id = 'ECB')
        @source_id = source_id
      end

      def parse(data, base_currency = 'EUR')
        # Simple stub for testing - returns empty array
        []
      end
    end
  end
end