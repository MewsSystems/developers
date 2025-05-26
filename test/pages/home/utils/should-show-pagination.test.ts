import { shouldShowPagination } from '../../../../src/pages/home/utils/should-show-pagination';

describe('shouldShowPagination', () => {
  it('should return true when totalPages is greater than 1', () => {
    expect(shouldShowPagination(2)).toBe(true);
    expect(shouldShowPagination(10)).toBe(true);
    expect(shouldShowPagination(100)).toBe(true);
  });

  it('should return false when totalPages is 1', () => {
    expect(shouldShowPagination(1)).toBe(false);
  });

  it('should return false when totalPages is 0', () => {
    expect(shouldShowPagination(0)).toBe(false);
  });

  it('should return false when totalPages is negative', () => {
    expect(shouldShowPagination(-1)).toBe(false);
    expect(shouldShowPagination(-10)).toBe(false);
  });
}); 