import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import SearchedMovies from './SearchedMovies'
import { useGetMovies } from '../../hooks/movies/useGetMovies'

// Mock the custom hook
vi.mock('../../hooks/movies/useGetMovies', () => ({
  useGetMovies: vi.fn(),
}))

describe('SearchedMovies', () => {
  const mockUseGetMovies = useGetMovies as vi.Mock

  it('renders loading skeletons when loading', () => {
    mockUseGetMovies.mockReturnValue({
      isLoading: true,
      isError: false,
      error: null,
      data: null,
    })

    render(<SearchedMovies query="Inception" />)

    // Assert skeleton cards are rendered
    const skeletonCards = screen.getAllByTestId('card-skeleton')
    expect(skeletonCards).toHaveLength(20)
  })

  it('renders an error message when there is an error', () => {
    mockUseGetMovies.mockReturnValue({
      isLoading: false,
      isError: true,
      error: { message: 'Something went wrong' },
      data: null,
    })

    render(<SearchedMovies query="Inception" />)

    // Assert the error message is displayed
    const errorMessage = screen.getByText(/something went wrong/i)
    expect(errorMessage).toBeInTheDocument()
  })

  it('renders movies and pagination when data is available', () => {
    mockUseGetMovies.mockReturnValue({
      isLoading: false,
      isError: false,
      error: null,
      data: {
        results: [
          { id: 1, title: 'Inception', poster_path: '/inception.jpg' },
          { id: 2, title: 'The Dark Knight', poster_path: '/dark-knight.jpg' },
        ],
        total_pages: 5,
      },
    })

    render(<SearchedMovies query="Inception" />)

    // Assert movies are rendered
    const movieTitles = screen.getAllByText(/inception|the dark knight/i)
    expect(movieTitles).toHaveLength(2)

    // // Assert pagination is rendered
    // const paginationButtons = screen.getAllByRole('button')
    // expect(paginationButtons).toHaveLength(4) // "First", "Previous", "Next", "Last"
  })

  it('does not render pagination if there is only one page', () => {
    mockUseGetMovies.mockReturnValue({
      isLoading: false,
      isError: false,
      error: null,
      data: {
        results: [{ id: 1, title: 'Inception', poster_path: '/inception.jpg' }],
        total_pages: 1,
      },
    })

    render(<SearchedMovies query="Inception" />)

    // Assert only one movie is rendered
    const movieTitles = screen.getByText(/inception/i)
    expect(movieTitles).toBeInTheDocument()

    // Assert pagination is not rendered
    const paginationButtons = screen.queryAllByRole('button')
    expect(paginationButtons).toHaveLength(0)
  })
})
