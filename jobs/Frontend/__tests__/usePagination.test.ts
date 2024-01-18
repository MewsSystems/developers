import { renderHook, act } from '@testing-library/react';
import usePagination from '@/hooks/usePagination';

describe('usePagination hook', () => {
  it('initializes currentPage and totalPages correctly', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 2, maxPages: 5 }));

    expect(result.current.page).toBe(2);
    expect(result.current.totalPages).toBe(5);
  });

  it('increments currentPage correctly', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 1, maxPages: 3 }));

    act(() => {
      result.current.increment();
    });

    expect(result.current.page).toBe(2);
  });

  it('does not increment currentPage beyond totalPages', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 3, maxPages: 3 }));

    act(() => {
      result.current.increment();
    });

    expect(result.current.page).toBe(3);
  });

  it('decrements currentPage correctly', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 2, maxPages: 3 }));

    act(() => {
      result.current.decrement();
    });

    expect(result.current.page).toBe(1);
  });

  it('does not decrement currentPage below 1', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 1, maxPages: 3 }));

    act(() => {
      result.current.decrement();
    });

    expect(result.current.page).toBe(1);
  });

  it('sets currentPage correctly', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 1, maxPages: 5 }));

    act(() => {
      result.current.setPage(3);
    });

    expect(result.current.page).toBe(3);
  });

  it('does not set currentPage outside of range', () => {
    const { result } = renderHook(() => usePagination({ initialPage: 1, maxPages: 5 }));

    act(() => {
      result.current.setPage(6);
    });

    expect(result.current.page).toBe(1);
  });

  it('updates totalPages when maxPages changes', () => {
    const { result, rerender } = renderHook(({ maxPages }) => usePagination({ initialPage: 1, maxPages }), {
      initialProps: { maxPages: 3 }
    });

    rerender({ maxPages: 5 });

    expect(result.current.totalPages).toBe(5);
  });
});
