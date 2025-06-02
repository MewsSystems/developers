import {act, renderHook} from '@testing-library/react';
import type {ChangeEvent} from 'react';
import {useSearchParams} from 'react-router-dom';

import type {SetURLSearchParams} from '../types';
import {useSearchInput} from '../useSearchInput';

jest.mock('react-router-dom', () => ({
  useSearchParams: jest.fn(),
}));

describe('useSearchInput', () => {
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
    it('should return empty string when no search parameter exists', () => {
      const {result} = renderHook(() => useSearchInput());
      expect(result.current.searchUrlParam).toBe('');
    });

    it('should return existing search parameter value', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({search: 'test query'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => useSearchInput());
      expect(result.current.searchUrlParam).toBe('test query');
    });

    it('should preserve other URL parameters', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({search: 'test', page: '1', sort: 'desc'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => useSearchInput());
      expect(result.current.searchUrlParam).toBe('test');
    });
  });

  describe('onSearchInputChange', () => {
    it('should update search parameter on input change', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: 'new search'},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('search')).toBe('new search');
    });

    it('should handle empty input by removing search parameter', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: ''},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('search')).toBe(false);
    });

    it('should preserve other parameters when updating search', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({page: '2', sort: 'asc'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: 'new search'},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('page')).toBe('2');
      expect(updatedParams.get('sort')).toBe('asc');
      expect(updatedParams.get('search')).toBe('new search');
    });
  });

  describe('clearSearch', () => {
    it('should remove search parameter', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({search: 'test query'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.clearSearch();
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('test query')).toBe(false);
    });

    it('should preserve other parameters when clearing search', () => {
      (useSearchParams as jest.Mock).mockReturnValue([
        createMockURLSearchParams({search: 'test', page: '2', sort: 'desc'}),
        mockSetSearchParams,
      ]);

      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.clearSearch();
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('page')).toBe('2');
      expect(updatedParams.get('sort')).toBe('desc');
      expect(updatedParams.has('search')).toBe(false);
    });
  });

  describe('edge cases', () => {
    it('should handle whitespace-only input', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: '   '},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('search')).toBe('   ');
    });

    it('should handle special characters in search', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: '!@#$%^&*()'},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('search')).toBe('!@#$%^&*()');
    });

    it('should handle very long search strings', () => {
      const longString = 'a'.repeat(1000);
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: longString},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.get('search')).toBe(longString);
    });

    it('should handle null value in event target', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: null as unknown as string},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('search')).toBe(false);
    });

    it('should handle undefined value in event target', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: undefined as unknown as string},
        } as ChangeEvent<HTMLInputElement>);
      });

      const updatedParams = mockSetSearchParams.mock.calls[0][0];
      expect(updatedParams.has('search')).toBe(false);
    });
  });

  describe('multiple operations', () => {
    it('should handle multiple search changes in sequence', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: 'first'},
        } as ChangeEvent<HTMLInputElement>);
      });

      expect(mockSetSearchParams.mock.calls[0][0].get('search')).toBe('first');

      act(() => {
        result.current.onSearchInputChange({
          target: {value: 'second'},
        } as ChangeEvent<HTMLInputElement>);
      });

      expect(mockSetSearchParams.mock.calls[1][0].get('search')).toBe('second');
    });

    it('should handle search change followed by clear', () => {
      const {result} = renderHook(() => useSearchInput());

      act(() => {
        result.current.onSearchInputChange({
          target: {value: 'test'},
        } as ChangeEvent<HTMLInputElement>);
      });

      expect(mockSetSearchParams.mock.calls[0][0].get('search')).toBe('test');

      act(() => {
        result.current.clearSearch();
      });

      expect(mockSetSearchParams.mock.calls[1][0].has('search')).toBe(false);
    });
  });
});
