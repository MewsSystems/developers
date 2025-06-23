# frozen_string_literal: true

require 'nokogiri'
require_relative '../providers/exchange_rate_provider'

class CzechExchangeRateProvider < ExchangeRateProvider
  private

  def api_url
    'https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml'
  end

  def source_provider
    'CZK'
  end

  def parse_response(response_body)
    parsed_data = XMLParser.parse_exchange_rates_to_json(
      response_body,
      xpath: '//radek',
      attributes: {
        currency_code: 'kod',
        rate: 'kurz',
        currency: 'mena'
      }
    )

    rates = parsed_data.map do |exchange|
      {
        currency_code: exchange[:currency_code],
        rate: exchange[:rate].to_s.gsub(',', '.').to_f,
        currency: exchange[:currency]
      }
    end

    { source_provider => rates }
  end
end
