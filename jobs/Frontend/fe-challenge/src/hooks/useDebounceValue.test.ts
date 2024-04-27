import { act, renderHook } from '@testing-library/react';
import useDebounceValue from './useDebounceValue';

describe('useDebounceValue()', () => {
  beforeEach(() => {
    vitest.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('should debounce the value', () => {
    const { result } = renderHook(() =>
      useDebounceValue<string>('value', 'initial-value', 200),
    );

    expect(result.current).toBe('initial-value');

    act(() => {
      vitest.advanceTimersByTime(400);
    });

    expect(result.current).toBe('value');
  });
});
