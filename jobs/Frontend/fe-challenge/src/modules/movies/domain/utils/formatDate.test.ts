import { formatDate } from '@/modules/movies/domain/utils/formatDate';

describe('formatDate', () => {
  test('should format date string to human format', () => {
    expect(formatDate('2009-07-08')).toEqual('Jul 8, 2009');
    expect(formatDate('2024-03-20')).toEqual('Mar 20, 2024');
  });

  test('should return "Invalid Date" if the date passed is not valid', () => {
    expect(formatDate('invalid-date')).toEqual('Invalid Date');
  });
});
