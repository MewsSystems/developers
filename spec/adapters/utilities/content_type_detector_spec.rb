require 'rails_helper'
require_relative '../../../app/adapters/utilities/content_type_detector'

RSpec.describe Adapters::Utilities::ContentTypeDetector do
  describe '.is_xml_content_type?' do
    it 'returns true for XML content types' do
      expect(described_class.is_xml_content_type?('text/xml')).to be true
      expect(described_class.is_xml_content_type?('application/xml')).to be true
      expect(described_class.is_xml_content_type?('application/xhtml+xml')).to be true
    end
    
    it 'is case-insensitive' do
      expect(described_class.is_xml_content_type?('TEXT/XML')).to be true
    end
    
    it 'returns false for non-XML content types' do
      expect(described_class.is_xml_content_type?('application/json')).to be false
      expect(described_class.is_xml_content_type?('text/plain')).to be false
    end
    
    it 'returns false for nil' do
      expect(described_class.is_xml_content_type?(nil)).to be false
    end
  end
  
  describe '.is_json_content_type?' do
    it 'returns true for JSON content types' do
      expect(described_class.is_json_content_type?('application/json')).to be true
      expect(described_class.is_json_content_type?('text/json')).to be true
    end
    
    it 'is case-insensitive' do
      expect(described_class.is_json_content_type?('APPLICATION/JSON')).to be true
    end
    
    it 'returns false for non-JSON content types' do
      expect(described_class.is_json_content_type?('application/xml')).to be false
      expect(described_class.is_json_content_type?('text/plain')).to be false
    end
    
    it 'returns false for nil' do
      expect(described_class.is_json_content_type?(nil)).to be false
    end
  end
  
  describe '.is_txt_content_type?' do
    it 'returns true for text content types' do
      expect(described_class.is_txt_content_type?('text/plain')).to be true
      expect(described_class.is_txt_content_type?('text/txt')).to be true
      expect(described_class.is_txt_content_type?('application/txt')).to be true
      expect(described_class.is_txt_content_type?('text/csv')).to be true
    end
    
    it 'is case-insensitive' do
      expect(described_class.is_txt_content_type?('TEXT/PLAIN')).to be true
    end
    
    it 'returns false for non-text content types' do
      expect(described_class.is_txt_content_type?('application/json')).to be false
      expect(described_class.is_txt_content_type?('application/xml')).to be false
    end
    
    it 'returns false for nil' do
      expect(described_class.is_txt_content_type?(nil)).to be false
    end
  end
  
  describe '.looks_like_xml?' do
    it 'returns true for XML-like content' do
      expect(described_class.looks_like_xml?('<?xml version="1.0"?><root></root>')).to be true
      expect(described_class.looks_like_xml?('<data><item>value</item></data>')).to be true
    end
    
    it 'returns false for non-XML content' do
      expect(described_class.looks_like_xml?('{"key": "value"}')).to be false
      expect(described_class.looks_like_xml?('plain text')).to be false
    end
    
    it 'returns false for nil' do
      expect(described_class.looks_like_xml?(nil)).to be false
    end
  end
  
  describe '.looks_like_json?' do
    it 'returns true for JSON-like content' do
      expect(described_class.looks_like_json?('{"key": "value"}')).to be true
      expect(described_class.looks_like_json?('[1, 2, 3]')).to be true
    end
    
    it 'returns false for invalid JSON even if it looks like JSON' do
      expect(described_class.looks_like_json?('{"key": value}')).to be false
      expect(described_class.looks_like_json?('[1, 2,]')).to be false
    end
    
    it 'returns false for non-JSON content' do
      expect(described_class.looks_like_json?('<data><item>value</item></data>')).to be false
      expect(described_class.looks_like_json?('plain text')).to be false
    end
    
    it 'returns false for nil' do
      expect(described_class.looks_like_json?(nil)).to be false
    end
  end
  
  describe '.detect_format' do
    it 'returns :xml for XML content' do
      expect(described_class.detect_format('<?xml version="1.0"?><root></root>')).to eq(:xml)
    end
    
    it 'returns :json for JSON content' do
      expect(described_class.detect_format('{"key": "value"}')).to eq(:json)
    end
    
    it 'returns :txt for other content' do
      expect(described_class.detect_format('plain text')).to eq(:txt)
    end
  end
  
  describe '.format_from_extension' do
    it 'returns correct format for known extensions' do
      expect(described_class.format_from_extension('json')).to eq(:json)
      expect(described_class.format_from_extension('xml')).to eq(:xml)
      expect(described_class.format_from_extension('txt')).to eq(:txt)
      expect(described_class.format_from_extension('text')).to eq(:txt)
      expect(described_class.format_from_extension('csv')).to eq(:txt)
    end
    
    it 'handles extensions with leading dot' do
      expect(described_class.format_from_extension('.json')).to eq(:json)
    end
    
    it 'is case-insensitive' do
      expect(described_class.format_from_extension('JSON')).to eq(:json)
    end
    
    it 'returns nil for unknown extensions' do
      expect(described_class.format_from_extension('doc')).to be_nil
    end
  end
  
  describe '.format_from_content_type' do
    it 'returns correct format for content types' do
      expect(described_class.format_from_content_type('application/json')).to eq(:json)
      expect(described_class.format_from_content_type('application/xml')).to eq(:xml)
      expect(described_class.format_from_content_type('text/plain')).to eq(:txt)
    end
    
    it 'returns nil for unknown content types' do
      expect(described_class.format_from_content_type('application/pdf')).to be_nil
    end
  end
end 