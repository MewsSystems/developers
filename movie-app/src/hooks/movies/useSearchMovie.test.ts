import { renderHook } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import { useSearchMovie } from '@/hooks/movies/useSearchMovie.ts'

vi.mock('react-router-dom', () => ({
    ...vi.importActual('react-router-dom'),
    useNavigate: () => vi.fn(),
    useLocation: () => vi.fn(),
}))

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
                    currentPage,
                    handleClear,
                    handlePageChange,
                    handleSearchChange,
                    isError,
                    isLoading,
                    results,
                    searchQuery,
                    searchRef,
                    totalPages,
                },
            },
        } = renderHook(() => useSearchMovie())

        expect(currentPage).toBe(1)
        expect(currentPage).toBe(1)
        expect(results).toHaveLength(2)
        expect(totalPages).toBe(3)
        expect(typeof handleSearchChange).toBe('function')
        expect(typeof handlePageChange).toBe('function')
        expect(typeof handleClear).toBe('function')
        expect(results).toHaveLength(2)
        expect(totalPages).toBe(3)
        expect(searchQuery).toBe('')
        expect(searchRef.current).toBe(null)
        expect(isError).toBe(false)
        expect(isLoading).toBe(false)
    })
})
