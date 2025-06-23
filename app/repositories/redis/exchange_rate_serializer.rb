require 'msgpack'
require 'json'

module RedisSupport
  # Handles serialization and deserialization of exchange rates
  class ExchangeRateSerializer
    def serialize(rates)
      rates_data = rates.map do |rate|
        {
          from: rate.from.code,
          to: rate.to.code,
          rate: rate.rate,
          date: rate.date.to_s
        }
      end

      MessagePack.pack(rates_data)
    end

    def deserialize(data)
      begin
        rates_data = MessagePack.unpack(data, symbolize_keys: true)
      rescue MessagePack::UnpackError
        rates_data = JSON.parse(data, symbolize_names: true)
      end

      rates_data.map do |rate_data|
        ExchangeRate.new(
          from: rate_data[:from],
          to: rate_data[:to],
          rate: rate_data[:rate],
          date: Date.parse(rate_data[:date])
        )
      end
    end
  end
end