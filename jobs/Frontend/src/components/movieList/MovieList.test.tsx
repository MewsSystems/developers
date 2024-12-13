import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import MovieList from './MovieList'
import { Movie } from '../../api/movieClient/movieClientTypes'

describe('MovieList', () => {
  it('renders a MovieCard for each movie in the list', () => {
    // Mock data
    const movies: Movie[] = [
      {
        id: 1,
        title: 'Inception',
        original_title: 'Inception',
        adult: false,
        poster_path: '/inception.jpg',
        release_date: '2010-07-16',
        overview:
          'A thief who steals corporate secrets through the use of dream-sharing technology.',
        backdrop_path: '/inception_backdrop.jpg',
        genre_ids: [28, 878, 12],
        original_language: 'en',
        popularity: 150.3,
        video: false,
        vote_average: 8.3,
        vote_count: 29000,
      },
      {
        id: 2,
        title: 'The Matrix',
        original_title: 'The Matrix',
        adult: false,
        poster_path: '/matrix.jpg',
        release_date: '1999-03-31',
        overview:
          'A computer hacker learns about the true nature of his reality and his role in the war against its controllers.',
        backdrop_path: '/matrix_backdrop.jpg',
        genre_ids: [28, 878],
        original_language: 'en',
        popularity: 180.5,
        video: false,
        vote_average: 8.7,
        vote_count: 22000,
      },
      {
        id: 3,
        title: 'Interstellar',
        original_title: 'Interstellar',
        adult: false,
        poster_path: '/interstellar.jpg',
        release_date: '2014-11-07',
        overview:
          'A team of explorers travel through a wormhole in space in an attempt to ensure humanity’s survival.',
        backdrop_path: '/interstellar_backdrop.jpg',
        genre_ids: [12, 18, 878],
        original_language: 'en',
        popularity: 200.1,
        video: false,
        vote_average: 8.6,
        vote_count: 23000,
      },
    ]

    // Render the component
    render(<MovieList movies={movies} />)

    // Assert that MovieCard is rendered for each movie
    movies.forEach((movie) => {
      expect(screen.getByText(movie.title)).toBeInTheDocument()
    })
  })

  it('renders nothing when the movies array is empty', () => {
    // Render the component with an empty list
    render(<MovieList movies={[]} />)

    // Assert that nothing is rendered
    expect(screen.queryByRole('listitem')).toBeNull()
  })
})
