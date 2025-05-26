import { formatLanguage } from '../../../../src/pages/movie-detail/utils/format-language';

describe('formatLanguage', () => {
  it('should convert language code to uppercase', () => {
    expect(formatLanguage('en')).toBe('EN');
    expect(formatLanguage('es')).toBe('ES');
    expect(formatLanguage('fr')).toBe('FR');
  });

  it('should handle empty string', () => {
    expect(formatLanguage('')).toBe('');
  });

  it('should handle already uppercase strings', () => {
    expect(formatLanguage('EN')).toBe('EN');
    expect(formatLanguage('ES')).toBe('ES');
  });

  it('should handle mixed case strings', () => {
    expect(formatLanguage('En')).toBe('EN');
    expect(formatLanguage('eS')).toBe('ES');
  });
}); 