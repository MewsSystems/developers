import { render } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import MovieDetailPage from './MovieDetailPage'

import { movieSearchResultsMock } from '@/tests/mocks/movieMocks.ts'

vi.mock('react-router-dom', () => ({
    ...vi.importActual('react-router-dom'),
    useNavigate: () => vi.fn(),
}))

vi.mock('@/hooks/movies/useMovieDetail.ts', () => ({
    __esModule: true,
    useMovieDetail: () => ({
        data: movieSearchResultsMock[0],
        isError: false,
        isLoading: false,
    }),
}))

vi.mock('@/hooks/movies/useSimilarMovies.ts', () => ({
    __esModule: true,
    useSimilarMovies: () => ({
        data: [movieSearchResultsMock[1]],
        isError: false,
        isLoading: false,
    }),
}))

describe('MovieDetailPage', () => {
    it('renders MovieDetailPage component with movie details and similar movies', () => {
        const { getByText, getByTestId } = render(<MovieDetailPage />)

        // Check if movie details are rendered, getByText because multiple same testIds
        expect(getByText('Movie Title')).toBeInTheDocument()
        expect(getByText('Movie overview')).toBeInTheDocument()
        expect(getByText('Movie tagline')).toBeInTheDocument()

        //Check getById to check presence and only occurrence
        expect(getByTestId('movie-origin')).toBeInTheDocument()
        expect(getByTestId('movie-genre')).toBeInTheDocument()

        // Check if similar movies are rendered
        expect(getByText('Adventure of a Lifetime')).toBeInTheDocument()
    })
})
