import { Movie } from '@/types/Movie'
import '@testing-library/jest-dom'
import { render, screen } from '@testing-library/react'
import MovieCard from './MovieCard'

describe('MovieCard', () => {
    const mockMovie: Movie = {
        id: 1,
        title: 'Test Movie',
        backdrop_path: '/test-path.jpg',
        release_date: '2023-01-01',
        overview: 'Test overview',
        vote_average: 7.5,
        poster_path: '/poster-path.jpg',
        original_language: 'en',
        tagline: 'Test tagline',
        popularity: 100,
    }

    it('renders movie title correctly', () => {
        render(<MovieCard movie={mockMovie} />)
        expect(screen.getByText('Test Movie')).toBeInTheDocument()
    })

    it('renders release year correctly', () => {
        render(<MovieCard movie={mockMovie} />)
        expect(screen.getByText('2023')).toBeInTheDocument()
    })

    it('renders "Untitled" when movie has no title', () => {
        const movieWithoutTitle = { ...mockMovie, title: '' }
        render(<MovieCard movie={movieWithoutTitle} />)
        expect(screen.getByText('Untitled')).toBeInTheDocument()
    })

    it('does not render year when release_date is missing', () => {
        const movieWithoutDate = { ...mockMovie, release_date: '' }
        render(<MovieCard movie={movieWithoutDate} />)
        expect(screen.queryByText('2023')).not.toBeInTheDocument()
    })

    it('renders with correct background image when backdrop_path exists', () => {
        render(<MovieCard movie={mockMovie} />)
        const movieDiv = screen
            .getByText('Test Movie')
            .closest('div')?.parentElement
        expect(movieDiv).toHaveStyle({
            backgroundImage: `url(https://image.tmdb.org/t/p/w500/test-path.jpg)`,
        })
    })

    it('renders with fallback background color when backdrop_path is missing', () => {
        const movieWithoutBackdrop = { ...mockMovie, backdrop_path: '' }
        render(<MovieCard movie={movieWithoutBackdrop} />)
        const movieDiv = screen
            .getByText('Test Movie')
            .closest('div')?.parentElement
        expect(movieDiv).toHaveStyle({
            backgroundColor: '#1f2937',
        })
    })

    it('links to the correct movie detail page', () => {
        render(<MovieCard movie={mockMovie} />)
        const link = screen.getByRole('link')
        expect(link).toHaveAttribute('href', '/1')
    })
})
