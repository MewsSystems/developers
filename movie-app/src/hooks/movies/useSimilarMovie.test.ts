import { renderHook } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import { useSimilarMovies } from '@/hooks/movies/useSimilarMovies.ts'

vi.mock('react-router-dom', () => ({
    useParams: () => ({ id: '123' }),
}))

vi.mock('@tanstack/react-query', () => ({
    useQuery: () => ({
        data: {
            page: 1,
            results: [
                { id: 1, title: 'Similar Movie 1' },
                { id: 2, title: 'Similar Movie 2' },
            ],
            total_pages: 3,
            total_results: 6,
        },
        isLoading: false,
        isError: false,
    }),
    keepPreviousData: vi.fn(),
}))

describe('useSimilarMovies', () => {
    it('returns similar movies and other properties', () => {
        const { result } = renderHook(() => useSimilarMovies())

        expect(result.current).toEqual({
            data: [
                { id: 1, title: 'Similar Movie 1' },
                { id: 2, title: 'Similar Movie 2' },
            ],
            isError: false,
            isLoading: false,
        })
    })
})
