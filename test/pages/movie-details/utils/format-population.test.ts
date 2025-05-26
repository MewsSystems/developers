import { formatPopularity } from '../../../../src/pages/movie-detail/utils/format-population';

describe('formatPopularity', () => {
  it('should format popularity with one decimal place', () => {
    expect(formatPopularity(123.456)).toBe('123.5');
    expect(formatPopularity(789.123)).toBe('789.1');
    expect(formatPopularity(42.999)).toBe('43.0');
  });

  it('should handle zero', () => {
    expect(formatPopularity(0)).toBe('0.0');
  });

  it('should handle negative numbers', () => {
    expect(formatPopularity(-123.456)).toBe('-123.5');
    expect(formatPopularity(-42.999)).toBe('-43.0');
  });

  it('should handle whole numbers', () => {
    expect(formatPopularity(100)).toBe('100.0');
    expect(formatPopularity(42)).toBe('42.0');
  });

  it('should handle very small numbers', () => {
    expect(formatPopularity(0.123)).toBe('0.1');
    expect(formatPopularity(0.001)).toBe('0.0');
  });

  it('should handle very large numbers', () => {
    expect(formatPopularity(999999.999)).toBe('1000000.0');
    expect(formatPopularity(1234567.89)).toBe('1234567.9');
  });
}); 