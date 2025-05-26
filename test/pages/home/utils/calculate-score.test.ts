import { calculateScore } from '../../../../src/pages/home/utils/calculate-score';

describe('calculateScore', () => {
  it('should calculate score correctly for whole numbers', () => {
    expect(calculateScore(8)).toBe(80);
    expect(calculateScore(10)).toBe(100);
    expect(calculateScore(5)).toBe(50);
  });

  it('should round down decimal scores', () => {
    expect(calculateScore(8.7)).toBe(87);
    expect(calculateScore(9.9)).toBe(99);
    expect(calculateScore(7.2)).toBe(72);
  });

  it('should handle zero score', () => {
    expect(calculateScore(0)).toBe(0);
  });

  it('should handle negative scores', () => {
    expect(calculateScore(-1)).toBe(-10);
    expect(calculateScore(-5.5)).toBe(-55);
  });

  it('should handle scores greater than 10', () => {
    expect(calculateScore(11)).toBe(110);
    expect(calculateScore(15.5)).toBe(155);
  });
}); 