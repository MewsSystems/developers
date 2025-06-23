require 'rails_helper'
require_relative '../../../app/services/utils/format_helper'

RSpec.describe Utils::FormatHelper do
  describe '.extract_date' do
    it 'extracts date from ISO format' do
      expect(described_class.extract_date('2023-11-15')).to eq(Date.new(2023, 11, 15))
    end

    it 'extracts date from European format' do
      expect(described_class.extract_date('15.11.2023')).to eq(Date.new(2023, 11, 15))
    end

    it 'returns nil for invalid date formats' do
      expect(described_class.extract_date('invalid')).to be_nil
    end

    it 'returns nil for nil input' do
      expect(described_class.extract_date(nil)).to be_nil
    end
  end

  describe '.parse_rate_value' do
    it 'parses numeric values' do
      expect(described_class.parse_rate_value(23.5)).to eq(23.5)
    end

    it 'parses string values with dot' do
      expect(described_class.parse_rate_value('23.5')).to eq(23.5)
    end

    it 'parses string values with comma' do
      expect(described_class.parse_rate_value('23,5')).to eq(23.5)
    end

    it 'parses hash values with rate key' do
      expect(described_class.parse_rate_value({'rate' => 23.5})).to eq(23.5)
    end

    it 'returns 0.0 for invalid inputs' do
      expect(described_class.parse_rate_value('invalid')).to eq(0.0)
    end
  end

  describe '.standardize_currency_code' do
    it 'converts to uppercase' do
      expect(described_class.standardize_currency_code('usd')).to eq('USD')
    end

    it 'trims whitespace' do
      expect(described_class.standardize_currency_code(' EUR ')).to eq('EUR')
    end

    it 'handles nil' do
      expect(described_class.standardize_currency_code(nil)).to eq('')
    end
  end

  describe '.parse_amount' do
    it 'parses numeric amount strings' do
      expect(described_class.parse_amount('100')).to eq(100)
    end

    it 'defaults to 1 for empty or nil input' do
      expect(described_class.parse_amount(nil)).to eq(1)
      expect(described_class.parse_amount('')).to eq(1)
    end

    it 'handles special cases for certain currencies' do
      expect(described_class.parse_amount('', 'JPY')).to eq(100)
    end
  end

  describe '.ensure_utf8_encoding' do
    it 'returns the input if already UTF-8' do
      utf8_string = 'UTF-8 string'.encode('UTF-8')
      expect(described_class.ensure_utf8_encoding(utf8_string)).to eq(utf8_string)
    end

    it 'converts ASCII to UTF-8' do
      ascii_string = 'ASCII string'.encode('US-ASCII')
      result = described_class.ensure_utf8_encoding(ascii_string, 'US-ASCII')
      expect(result.encoding.name).to eq('UTF-8')
    end
  end

  describe '::ContentTypeDetection' do
    let(:detection) { Utils::FormatHelper::ContentTypeDetection }

    describe '.is_xml_content_type?' do
      it 'recognizes XML content types' do
        expect(detection.is_xml_content_type?('text/xml')).to be true
        expect(detection.is_xml_content_type?('application/xml')).to be true
      end

      it 'returns false for non-XML content types' do
        expect(detection.is_xml_content_type?('application/json')).to be false
        expect(detection.is_xml_content_type?(nil)).to be false
      end
    end

    describe '.is_json_content_type?' do
      it 'recognizes JSON content types' do
        expect(detection.is_json_content_type?('application/json')).to be true
        expect(detection.is_json_content_type?('text/json')).to be true
      end

      it 'returns false for non-JSON content types' do
        expect(detection.is_json_content_type?('text/xml')).to be false
        expect(detection.is_json_content_type?(nil)).to be false
      end
    end

    describe '.looks_like_xml?' do
      it 'recognizes XML content' do
        expect(detection.looks_like_xml?('<?xml version="1.0"?>')).to be true
        expect(detection.looks_like_xml?('<root><child>value</child></root>')).to be true
      end

      it 'returns false for non-XML content' do
        expect(detection.looks_like_xml?('{"key": "value"}')).to be false
        expect(detection.looks_like_xml?(nil)).to be false
      end
    end

    describe '.looks_like_json?' do
      it 'recognizes JSON content' do
        expect(detection.looks_like_json?('{"key": "value"}')).to be true
        expect(detection.looks_like_json?('[1, 2, 3]')).to be true
      end

      it 'returns false for non-JSON content' do
        expect(detection.looks_like_json?('<root><child>value</child></root>')).to be false
        expect(detection.looks_like_json?(nil)).to be false
      end
    end
  end
end