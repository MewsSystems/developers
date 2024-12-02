import { describe, expect, vi, test } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MovieDetailView } from './MovieDetailView'
import type { MovieDetailPageProps } from './types'
import type { MovieDetail } from '../../types'

// Mock any external components and icons if necessary
vi.mock('@mui/icons-material', () => ({
    ArrowBack: () => <div>ArrowBackIcon</div>,
    Home: () => <div>HomeIcon</div>,
}))

vi.mock('../../components/MoviePoster', () => ({
    MoviePoster: () => <div>MoviePoster</div>,
}))

vi.mock('./components/TopSummary', () => ({
    TopSummary: () => <div>TopSummary</div>,
}))

const mockProps: MovieDetailPageProps = {
    detailData: undefined,
    isLoading: false,
    isError: false,
    navigateBack: vi.fn(),
    navigateHome: vi.fn(),
    isPreviousPageAvailable: false,
    similarMovieData: undefined,
    isSimilarLoading: false,
    isSimilarError: false,
    prefetchSimilarMovieData: vi.fn(),
}

describe('MovieDetailView', () => {
    test('renders correctly when data is loading', () => {
        render(
            <MovieDetailView
                {...mockProps}
                isLoading={true}
            />,
        )

        const posterSkeleton = screen.getByTestId('Skeleton')
        expect(posterSkeleton).toBeInTheDocument()
    })

    test('renders the movie details when data is available and not loading', () => {
        const detailData: MovieDetail = {
            id: 1,
            genres: [{ id: 1, name: 'Action' }],
            original_language: 'en',
            popularity: 80,
            release_date: '2021-01-01',
            runtime: 120,
            vote_average: 8,
            vote_count: 200,
            title: 'Example Movie',
            poster_path: '/path/to/image',
            overview: 'A brief overview of the movie.',
        }

        render(
            <MovieDetailView
                {...mockProps}
                detailData={detailData}
            />,
        )

        expect(screen.getByText(/example movie/i)).toBeInTheDocument()
        expect(
            screen.getByText(/a brief overview of the movie/i),
        ).toBeInTheDocument()
    })
})
