import { render } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import SearchMovieContent from './SearchMovieContent'

import { movieSearchResultsMock } from '@/tests/mocks/movieMocks.ts'

const handleClear = vi.fn()

const mockedUsedNavigate = vi.fn()

vi.mock('react-router-dom', () => ({
    ...vi.importActual('react-router-dom'),
    useNavigate: () => mockedUsedNavigate,
}))

describe('SearchMovieContent', () => {
    it('renders loading state', () => {
        const { getByRole } = render(
            <SearchMovieContent handleClear={handleClear} isLoading />
        )
        expect(getByRole('progressbar')).toBeInTheDocument()
    })

    it('renders error state', () => {
        const { getByText, getByRole } = render(
            <SearchMovieContent isError handleClear={handleClear} />
        )

        expect(
            getByText('Oops! Something went wrong. Please try again later.')
        ).toBeInTheDocument()
        expect(
            getByRole('button', { name: 'Go to homepage' })
        ).toBeInTheDocument()
    })

    it('renders empty results state', () => {
        const { getByText, queryByText } = render(
            <SearchMovieContent results={[]} handleClear={vi.fn()} />
        )

        expect(
            getByText('Sorry we could not find anything for you.')
        ).toBeInTheDocument()
        expect(queryByText('Movie 1')).not.toBeInTheDocument()
        expect(queryByText('Movie 2')).not.toBeInTheDocument()
    })

    it('renders movie tiles when results are available', () => {
        const { queryByText } = render(
            <SearchMovieContent
                results={movieSearchResultsMock}
                handleClear={handleClear}
            />
        )
        expect(queryByText('Movie 1')).not.toBeInTheDocument()
        expect(queryByText('Movie 2')).not.toBeInTheDocument()
    })
})
