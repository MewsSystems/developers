import { render, fireEvent, screen } from '@testing-library/react'
import MovieItem from './MovieItem.tsx'
import { Movie } from '../../domain/interfaces/movie.interface.ts'

const mockedUsedNavigate = jest.fn()

jest.mock('react-router-dom', () => ({
    ...(jest.requireActual('react-router-dom') as any),
    useNavigate: () => mockedUsedNavigate,
}))

const movie: Movie = {
    id: 1,
    title: 'Movie Title',
    overview: 'Movie Overview',
    release_date: '2024-01-01',
    poster_path: 'path/to/image.jpg',
    vote_average: 7,
    vote_count: 100,
    adult: false,
    backdrop_path: 'path/to/backdrop.jpg',
    genre_ids: [1, 2, 3],
    original_language: 'en',
    original_title: 'Movie Original Title',
    popularity: 70,
    video: false,
}

describe('MovieItem', () => {
    it('should redirect when clicking on a MovieItem', async () => {
        render(<MovieItem movie={movie} />)
        const movieTitle = await screen.findByText('Movie Title')

        expect(movieTitle).toBeTruthy()

        fireEvent.click(movieTitle)
        expect(mockedUsedNavigate).toHaveBeenCalledWith('/movie/1')
    })
})
