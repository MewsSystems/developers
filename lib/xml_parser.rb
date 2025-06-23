# frozen_string_literal: true

require 'nokogiri'

class XMLParser
  def self.parse_exchange_rates_to_json(xml_body, xpath:, attributes:)
    doc = Nokogiri::XML(xml_body)
    parsed_data = []

    doc.xpath(xpath).each do |node|
      parsed_entry = {}
      attributes.each do |key, attr_name|
        parsed_entry[key] = node.attr(attr_name)
      end
      parsed_data << parsed_entry
    end

    parsed_data
  end
end
