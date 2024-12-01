import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import MovieCard from './MovieCard'
import movieClient from '../../api/movieClient/movieClient'

// Mock the movieClient and its method
vi.mock('../../api/movieClient/movieClient', () => ({
  buildMoviePosterUrl: vi.fn(),
}))

describe('MovieCard', () => {
  const mockMovie = {
    id: 1,
    title: 'Inception',
    overview: 'A thief who steals corporate secrets...',
    poster_path: '/inception-poster.jpg',
  }

  it('renders the movie card with the correct details', () => {
    // Mock the return value for the poster URL
    movieClient.buildMoviePosterUrl.mockReturnValue(
      'http://example.com/inception-poster.jpg'
    )

    render(
      <MemoryRouter>
        <MovieCard movie={mockMovie} />
      </MemoryRouter>
    )

    // Assert that the title is displayed
    const title = screen.getByRole('heading', { name: /inception/i })
    expect(title).toBeInTheDocument()

    // Assert that the overview text is displayed
    const overview = screen.getByText(/a thief who steals corporate secrets/i)
    expect(overview).toBeInTheDocument()

    // Assert that the background image includes the mocked poster URL
    const poster = screen.getByTestId('movie-poster')
    expect(poster).toHaveStyle(
      'background-image: linear-gradient(rgba(255,255,255,.6), rgba(255,255,255,.5)), url("http://example.com/inception-poster.jpg")'
    )

    // Assert "More Details" button and link
    const button = screen.getByRole('button', { name: /more details/i })
    expect(button).toBeNull

    const link = screen.getByRole('link', { name: /more details/i })
    expect(link).toHaveAttribute('href', '/movie/1')
  })
})
