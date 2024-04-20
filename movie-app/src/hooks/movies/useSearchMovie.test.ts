import { renderHook } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import { MoviesSearchApiResponse } from '@/hooks/movies/types.ts'
import { useSearchMovie } from '@/hooks/movies/useSearchMovie.ts'

vi.mock('@tanstack/react-query', () => ({
    useQuery: () => ({
        data: {
            page: 1,
            results: [
                { id: 1, title: 'Movie 1' },
                { id: 2, title: 'Movie 2' },
            ],
            total_pages: 3,
            total_results: 6,
        },
        isLoading: false,
        isError: false,
    }),
    keepPreviousData: vi.fn(),
}))

describe('useSearchMovie', () => {
    it('returns search results and other properties', () => {
        const {
            result: {
                current: {
                    page,
                    currentPage,
                    results,
                    totalPages,
                    totalResults,
                    handleSearch,
                    handlePageChange,
                    handleClear,
                    data,
                    currentSearch,
                    searchRef,
                    isError,
                    isLoading,
                },
            },
        } = renderHook(() => useSearchMovie())

        expect(page).toBe(1)
        expect(currentPage).toBe(1)
        expect(results).toHaveLength(2)
        expect(totalPages).toBe(3)
        expect(totalResults).toBe(6)
        expect(typeof handleSearch).toBe('function')
        expect(typeof handlePageChange).toBe('function')
        expect(typeof handleClear).toBe('function')
        expect((data as MoviesSearchApiResponse).page).toBe(1)
        expect((data as MoviesSearchApiResponse).results).toHaveLength(2)
        expect((data as MoviesSearchApiResponse).total_pages).toBe(3)
        expect((data as MoviesSearchApiResponse).total_results).toBe(6)
        expect(currentSearch).toBe('')
        expect(searchRef.current).toBe(null)
        expect(isError).toBe(false)
        expect(isLoading).toBe(false)
    })
})
