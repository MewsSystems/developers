require 'nokogiri'
require_relative '../base/base_adapter'
require_relative '../../services/utils/format_helper'

class XmlAdapter < BaseAdapter
  CONTENT_TYPES = Utils::FormatHelper::ContentTypeDetection::XML_CONTENT_TYPES
  
  def supports_content_type?(content_type)
    Utils::FormatHelper::ContentTypeDetection.is_xml_content_type?(content_type)
  end
  
  def supports_content?(content)
    Utils::FormatHelper::ContentTypeDetection.looks_like_xml?(content)
  end
  
  # Parse XML exchange rate data
  # Expected format:
  # <exchangeRates date="YYYY-MM-DD">
  #   <rate>
  #     <code>USD</code>
  #     <value>21.123</value>
  #     <amount>1</amount>
  #   </rate>
  #   ...
  # </exchangeRates>
  # 
  # Alternative format:
  # <rates>
  #   <currency code="USD" amount="1">21.123</currency>
  #   ...
  # </rates>
  def perform_parse(data, base_currency)
    begin
      # Parse XML with Nokogiri
      doc = Nokogiri::XML(data) { |config| config.strict }
      
      # Try to extract date
      date_attr = doc.root['date']
      date = date_attr ? extract_date(date_attr) : Date.today
    rescue => e
      raise ParseError, "Invalid XML: #{e.message}"
    end
    
    # Try different possible XML structures
    rates = try_rate_elements_format(doc, base_currency, date) ||
            try_currency_elements_format(doc, base_currency, date) ||
            try_tag_as_currency_format(doc, base_currency, date) ||
            []
    
    rates
  end
  
  private
  
  # Format 1: <rate> elements with child elements
  def try_rate_elements_format(doc, base_currency, date)
    rates = []
    
    doc.css('rate').each do |rate_elem|
      begin
        code_elem = rate_elem.at_css('code')
        value_elem = rate_elem.at_css('value')
        amount_elem = rate_elem.at_css('amount')
        
        next unless code_elem && value_elem
        
        currency_code = standardize_currency_code(code_elem.text)
        rate_value = parse_rate_value(value_elem.text)
        amount = amount_elem ? amount_elem.text.to_i : 1
        
        # Skip invalid codes
        next if currency_code.empty? || currency_code.length != 3
        
        # Create exchange rate object
        rates << create_exchange_rate(base_currency, currency_code, rate_value, amount, date)
      rescue => e
        # Skip elements with errors
        next
      end
    end
    
    rates.empty? ? nil : rates
  end
  
  # Format 2: <currency> elements with attributes
  def try_currency_elements_format(doc, base_currency, date)
    rates = []
    
    doc.css('currency').each do |currency_elem|
      begin
        currency_code = standardize_currency_code(currency_elem['code'])
        rate_value = parse_rate_value(currency_elem.text)
        amount = currency_elem['amount'] ? currency_elem['amount'].to_i : 1
        
        # Skip invalid codes
        next if currency_code.empty? || currency_code.length != 3
        
        # Create exchange rate object
        rates << create_exchange_rate(base_currency, currency_code, rate_value, amount, date)
      rescue => e
        # Skip elements with errors
        next
      end
    end
    
    rates.empty? ? nil : rates
  end
  
  # Format 3: Element tags are currency codes
  def try_tag_as_currency_format(doc, base_currency, date)
    rates = []
    
    doc.root.children.each do |elem|
      next unless elem.element?
      
      begin
        # Use the tag name as currency code
        currency_code = standardize_currency_code(elem.name)
        rate_value = parse_rate_value(elem.text)
        
        # Skip invalid codes
        next if currency_code.empty? || currency_code.length != 3
        next if rate_value.zero?
        
        # Create exchange rate object
        rates << create_exchange_rate(base_currency, currency_code, rate_value, 1, date)
      rescue => e
        # Skip elements with errors
        next
      end
    end
    
    rates.empty? ? nil : rates
  end
end 