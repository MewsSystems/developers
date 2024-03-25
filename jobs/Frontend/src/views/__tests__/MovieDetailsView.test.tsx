import { screen, render } from '@testing-library/react'
import { MovieDetailsView } from '@/views/MovieDetailsView'
import React from 'react'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { Movie } from '@/types'

vi.mock('@/services', () => ({
  movieService: {
    getMovieDetails: vi.fn().mockResolvedValue({
      id: 123,
      overview: 'Overview',
      poster_path: 'Image',
      release_date: '1978-11-15',
      title: 'Title',
      vote_count: 9,
      vote_average: 7.5,
    }) as Partial<Movie>,
  },
}))

describe('Movie Details View', () => {
  it('should render the movie details', async () => {
    const testId = '123'

    render(
      <MemoryRouter initialEntries={[`/movie/${testId}`]}>
        <Routes>
          <Route path="/movie/:id" element={<MovieDetailsView />} />
        </Routes>
      </MemoryRouter>
    )

    const overview = await screen.findByText(/Overview/i)
    const title = await screen.findByText(/Title/i)
    const release_date = await screen.findByText(/11\/15\/1978/i)
    const stars = await screen.findAllByTestId(/StarIcon/i)
    const vote_average = await screen.findByTestId(/voteCount/i)

    expect(overview).toBeInTheDocument()
    expect(title).toBeInTheDocument()
    expect(release_date).toBeInTheDocument()
    expect(stars.length).toBe(4)
    expect(vote_average).toBeInTheDocument()
  })
})
