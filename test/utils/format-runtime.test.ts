import { formatRuntime } from '../../src/utils/format-runtime';

describe('formatRuntime', () => {
  it('should format runtime with hours and minutes', () => {
    expect(formatRuntime(90)).toBe('1h 30m');
    expect(formatRuntime(120)).toBe('2h 0m');
    expect(formatRuntime(45)).toBe('0h 45m');
  });

  it('should handle zero minutes', () => {
    expect(formatRuntime(0)).toBe('0h 0m');
  });

  it('should handle minutes less than an hour', () => {
    expect(formatRuntime(30)).toBe('0h 30m');
    expect(formatRuntime(59)).toBe('0h 59m');
  });

  it('should handle exact hours', () => {
    expect(formatRuntime(60)).toBe('1h 0m');
    expect(formatRuntime(120)).toBe('2h 0m');
    expect(formatRuntime(180)).toBe('3h 0m');
  });

  it('should handle long runtimes', () => {
    expect(formatRuntime(150)).toBe('2h 30m');
    expect(formatRuntime(210)).toBe('3h 30m');
  });

  it('should handle edge cases', () => {
    expect(formatRuntime(1)).toBe('0h 1m');
    expect(formatRuntime(59)).toBe('0h 59m');
    expect(formatRuntime(61)).toBe('1h 1m');
  });
}); 