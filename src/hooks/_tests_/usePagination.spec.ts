import {renderHook, act} from '@testing-library/react';
import {useSearchParams} from 'react-router-dom';
import {usePagination} from '../usePagination';
import type {SetURLSearchParams} from '../types';

jest.mock('react-router-dom', () => ({
  useSearchParams: jest.fn(),
}));

describe('usePagination', () => {
  const createMockURLSearchParams = (initialParams: Record<string, string> = {}) => {
    const params = new URLSearchParams();
    Object.entries(initialParams).forEach(([key, value]) => {
      params.set(key, value);
    });

    return params;
  };

  let mockSetSearchParams: jest.Mock<SetURLSearchParams>;

  beforeEach(() => {
    mockSetSearchParams = jest.fn();
    (useSearchParams as jest.Mock).mockReturnValue([
      createMockURLSearchParams(),
      mockSetSearchParams,
    ]);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('initial state', () => {
    it('should return page 1 when no page parameter exists', () => {
      const {result} = renderHook(() => usePagination());
      expect(result.current.page).toBe(1);
    });

    it('should return correct page from URL parameters', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '5'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => usePagination());
      expect(result.current.page).toBe(5);
    });

    it('should handle invalid page parameter and return 1', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: 'invalid'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => usePagination());
      expect(result.current.page).toBe(1);
    });
  });

  describe('setPage', () => {
    it('should update URL parameters with new page', () => {
      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.setPage(3);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('page')).toBe('3');
    });

    it('should preserve existing URL parameters when setting page', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({search: 'test', page: '1'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.setPage(2);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('search')).toBe('test');
      expect(updatedParams.get('page')).toBe('2');
    });
  });

  describe('onPageChange', () => {
    it('should remove page parameter when changing to page 1', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '2'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.onPageChange(1);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('page')).toBe(false);
    });

    it('should set page parameter for pages greater than 1', () => {
      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.onPageChange(3);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('page')).toBe('3');
    });

    it('should handle multiple page changes', () => {
      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.onPageChange(2);
      });

      let updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('page')).toBe('2');

      act(() => {
        result.current.onPageChange(1);
      });

      updatedParams = mockSetSearchParams.mock.calls[1][0];
      expect(updatedParams.has('page')).toBe(false);

      act(() => {
        result.current.onPageChange(3);
      });

      updatedParams = mockSetSearchParams.mock.calls[2][0];
      expect(updatedParams.get('page')).toBe('3');
    });

    it('should preserve other URL parameters when changing pages', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({search: 'movie', sort: 'desc', page: '1'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.onPageChange(2);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];

      expect(updatedParams.get('search')).toBe('movie');
      expect(updatedParams.get('sort')).toBe('desc');
      expect(updatedParams.get('page')).toBe('2');
    });
  });

  describe('edge cases', () => {
    it('should handle negative page numbers by setting to page 1', () => {
      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.onPageChange(-1);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('page')).toBe(false);
    });

    it('should handle zero page number by setting to page 1', () => {
      const {result} = renderHook(() => usePagination());

      act(() => {
        result.current.onPageChange(0);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('page')).toBe(false);
    });
  });
});
