import {act, renderHook} from '@testing-library/react';
import {useSearchParams} from 'react-router-dom';
import {usePaginationWithUrlSync} from '../usePaginationWithUrlSync';
import type {SetURLSearchParams} from '../types';

jest.mock('react-router-dom', () => ({
  useSearchParams: jest.fn(),
}));

describe('usePaginationWithUrlSync', () => {
  let mockSetSearchParams: jest.Mock<SetURLSearchParams>;

  const createMockURLSearchParams = (initialParams: Record<string, string> = {}) => {
    const params = new URLSearchParams();
    Object.entries(initialParams).forEach(([key, value]) => {
      params.set(key, value);
    });
    return params;
  };

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
      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );
      expect(result.current.currentPage).toBe(1);
    });

    it('should return existing page parameter value', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '3'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );
      expect(result.current.currentPage).toBe(3);
    });

    it('should preserve other URL parameters', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '2', search: 'test', sort: 'desc'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );
      expect(result.current.currentPage).toBe(2);
    });
  });

  describe('setPage', () => {
    it('should update page parameter when setting a page > 1', () => {
      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );

      act(() => {
        result.current.setPage(3);
      });

      const newParams = new URLSearchParams();
      newParams.set('page', '3');
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });

    it('should remove page parameter when setting page to 1', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '3'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );

      act(() => {
        result.current.setPage(1);
      });

      const newParams = new URLSearchParams();
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });

    it('should preserve other parameters when updating page', () => {
      const initialParams = createMockURLSearchParams({
        page: '2',
        search: 'test',
        sort: 'desc',
      });
      (useSearchParams as jest.Mock).mockReturnValue([initialParams, mockSetSearchParams]);

      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );

      act(() => {
        result.current.setPage(4);
      });

      const newParams = new URLSearchParams(initialParams);
      newParams.set('page', '4');
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });
  });

  describe('URL synchronization', () => {
    it('should remove page parameter when search query is empty', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '2'}),
        mockSetSearchParams,
      ]);

      renderHook(() => usePaginationWithUrlSync({totalPages: 5, searchQuery: ''}));

      const newParams = new URLSearchParams();
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });

    it('should add page=1 when total pages > 1 and no page parameter exists', () => {
      renderHook(() => usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}));

      const newParams = new URLSearchParams();
      newParams.set('page', '1');
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });

    it('should remove page parameter when total pages <= 1', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '2'}),
        mockSetSearchParams,
      ]);

      renderHook(() => usePaginationWithUrlSync({totalPages: 1, searchQuery: 'test'}));

      const newParams = new URLSearchParams();
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });

    it('should not modify URL if conditions for changes are not met', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '2'}),
        mockSetSearchParams,
      ]);

      renderHook(() => usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}));

      expect(mockSetSearchParams).not.toHaveBeenCalled();
    });
  });

  describe('edge cases', () => {
    it('should handle non-numeric page parameter', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: 'invalid'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );
      expect(result.current.currentPage).toBe(1);
    });

    it('should handle negative page numbers in setPage', () => {
      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );

      act(() => {
        result.current.setPage(-1);
      });

      const newParams = new URLSearchParams();
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });

    it('should handle zero page number in setPage', () => {
      const {result} = renderHook(() =>
        usePaginationWithUrlSync({totalPages: 5, searchQuery: 'test'}),
      );

      act(() => {
        result.current.setPage(0);
      });

      const newParams = new URLSearchParams();
      expect(mockSetSearchParams).toHaveBeenCalledWith(newParams);
    });
  });
});
