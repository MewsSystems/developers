import { render } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import MovieTile from '@/components/movie/MovieTile.tsx'
import { movieSearchResultsMock } from '@/tests/mocks/movieMocks.ts'

const mockedUsedNavigate = vi.fn()

vi.mock('react-router-dom', () => ({
    ...vi.importActual('react-router-dom'),
    useNavigate: () => mockedUsedNavigate,
}))

describe('MovieTile', () => {
    it('renders movie information correctly', () => {
        const { getByTestId } = render(
            <MovieTile {...movieSearchResultsMock[0]} />
        )

        // Check if movie title, overview are present
        expect(getByTestId('movie-title')).toBeInTheDocument()
        expect(getByTestId('movie-overview')).toBeInTheDocument()
    })

    it('renders movie information for when isDetail=true', () => {
        const { getByTestId, queryByTestId } = render(
            <MovieTile {...movieSearchResultsMock[0]} />
        )

        // Check if movie title, overview are present
        expect(getByTestId('movie-title')).toBeInTheDocument()
        expect(getByTestId('movie-overview')).toBeInTheDocument()

        // Check if overview is having overflow styles
        expect(queryByTestId('movie-overview')).toHaveStyle('overflow: hidden;')

        // Check if movie tile is not rendering components for detail
        expect(queryByTestId('movie-tagline')).not.toBeInTheDocument()
        expect(queryByTestId('movie-genre')).not.toBeInTheDocument()
    })

    it('renders movie information for when isDetail=false', () => {
        const { getByTestId, queryByTestId } = render(
            <MovieTile isDetail {...movieSearchResultsMock[0]} />
        )

        //Check presence of elements
        expect(getByTestId('movie-title')).toBeInTheDocument()
        expect(getByTestId('movie-overview')).toBeInTheDocument()
        expect(queryByTestId('movie-genre')).toBeInTheDocument()
        expect(queryByTestId('movie-tagline')).toBeInTheDocument()

        //Check overview does show full text
        expect(queryByTestId('movie-overview')).not.toHaveStyle(
            'overflow: hidden;'
        )
    })
})
