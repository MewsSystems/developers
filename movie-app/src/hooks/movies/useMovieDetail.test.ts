import { describe, expect, it, vi } from 'vitest'

import { useMovieDetail } from './useMovieDetail'

const mockParams = { id: '123' }
vi.mock('react-router-dom', () => ({
    useParams: () => mockParams,
}))

vi.mock('@tanstack/react-query', () => ({
    useQuery: vi.fn().mockReturnValue({
        data: { id: 123, title: 'Dummy Movie' },
        isError: false,
        isLoading: false,
    }),
    keepPreviousData: vi.fn(),
}))

describe('useMovieDetail', () => {
    it('returns movie details', async () => {
        const { data, isError, isLoading } = useMovieDetail()

        expect(data).toEqual({ id: 123, title: 'Dummy Movie' })
        expect(isError).toBe(false)
        expect(isLoading).toBe(false)
    })
})
