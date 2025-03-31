require_relative '../base/base_adapter'
require_relative '../../services/utils/format_helper'

class TxtAdapter < BaseAdapter
  CONTENT_TYPES = Utils::FormatHelper::ContentTypeDetection::TXT_CONTENT_TYPES
  
  def supports_content_type?(content_type)
    Utils::FormatHelper::ContentTypeDetection.is_txt_content_type?(content_type)
  end
  
  def supports_content?(content)
    # If it's not XML or JSON, it's probably text
    !Utils::FormatHelper::ContentTypeDetection.looks_like_xml?(content) &&
    !Utils::FormatHelper::ContentTypeDetection.looks_like_json?(content) &&
    content && !content.to_s.strip.empty?
  end
  
  # Parse CNB-style TXT format data
  # Format example:
  # Date: 01.01.2023
  # Country|Currency|Amount|Code|Rate
  # Australia|dollar|1|AUD|15.123
  # ...
  def perform_parse(data, base_currency)
    lines = data.strip.split("\n")
    
    # Skip empty lines
    lines = lines.select { |line| !line.strip.empty? }
    return [] if lines.empty?
    
    # Try to extract date from the first line
    date = extract_date(lines.first)
    
    # Skip header lines (typically 2 in CNB format)
    data_lines = lines[2..-1] || []
    return [] if data_lines.empty?
    
    # Parse each data line
    rates = []
    
    data_lines.each do |line|
      # Normalize delimiters (could be |, tabs, or multiple spaces)
      normalized_line = line.strip.gsub(/\s+\|\s+|\t+|\s{2,}/, '|')
      parts = normalized_line.split('|')
      
      # Minimum expected parts: country, currency name, amount, code, rate
      next if parts.length < 5
      
      # Extract components (positions based on CNB format)
      country = parts[0].strip
      currency_name = parts[1].strip
      amount = parse_amount(parts[2], parts[3])
      currency_code = standardize_currency_code(parts[3].strip)
      rate_value = parse_rate_value(parts[4])
      
      # Skip invalid codes
      next if currency_code.empty? || currency_code.length != 3
      
      # Create exchange rate object
      rates << create_exchange_rate(base_currency, currency_code, rate_value, amount, date)
    end
    
    rates
  end
  
  private
  
  # Extract date from a string in various formats
  # @param line [String] Line to extract date from
  # @return [Date] Extracted date or today's date if not found
  def extract_date(line)
    # Try to match DD.MM.YYYY format
    if match = line.match(/(\d{1,2})\.(\d{1,2})\.(\d{4})/)
      day, month, year = match.captures
      return Date.new(year.to_i, month.to_i, day.to_i)
    end
    
    # Try to match YYYY-MM-DD format
    if match = line.match(/(\d{4})-(\d{1,2})-(\d{1,2})/)
      year, month, day = match.captures
      return Date.new(year.to_i, month.to_i, day.to_i)
    end
    
    # Default to today if no date found
    Date.today
  end
end 