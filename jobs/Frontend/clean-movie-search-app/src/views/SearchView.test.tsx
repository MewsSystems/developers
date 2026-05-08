import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import { ThemeProvider } from 'styled-components';
import { lightTheme } from '../theme/themes';
import { SearchView } from './SearchView';
import { useMovieContext } from '../context/MovieContext';
import { Movie } from '../api';

jest.mock('../context/MovieContext');

describe('SearchView', () => {
  const mockMovies: Movie[] = [
    {
      id: 1,
      title: 'Movie 1',
      overview: 'Movie 1 overview',
      poster_path: null,
      release_date: '2024-01-01',
      vote_average: 6.5,
    },
    {
      id: 2,
      title: 'Movie 2',
      overview: 'Movie 2 overview',
      poster_path: '/poster2.jpg',
      release_date: '2023-10-10',
      vote_average: 7.2,
    },
  ];

  const mockMovieContext = {
    query: 'new search',
    setQuery: jest.fn(),
    movies: mockMovies,
    loading: false,
    paginationLoading: false,
    error: '',
    page: 1,
    totalPages: 2,
    handleSearch: jest.fn(),
    loadMore: jest.fn(),
  };

  beforeEach(() => {
    (useMovieContext as jest.Mock).mockReturnValue(mockMovieContext);
  });

  it('calls handleSearch with the correct query', async () => {
    render(
      <BrowserRouter
        future={{
          v7_startTransition: true,
          v7_relativeSplatPath: true,
        }}
      >
        <ThemeProvider theme={lightTheme}>
          <SearchView />
        </ThemeProvider>
      </BrowserRouter>
    );

    const inputElement = screen.getByRole('textbox');
    fireEvent.change(inputElement, { target: { value: 'new search' } });

    // Wait for the debounce and the effect to run
    await waitFor(() => {
      expect(mockMovieContext.handleSearch).toHaveBeenCalledWith(
        'new search',
        1
      );
    });
  });

  it('calls loadMore when the button is clicked', () => {
    render(
      <BrowserRouter
        future={{
          v7_startTransition: true,
          v7_relativeSplatPath: true,
        }}
      >
        <ThemeProvider theme={lightTheme}>
          <SearchView />
        </ThemeProvider>
      </BrowserRouter>
    );

    const loadMoreButton = screen.getByRole('button', {
      name: /load more movies/i,
    });
    fireEvent.click(loadMoreButton);

    expect(mockMovieContext.loadMore).toHaveBeenCalledTimes(1);
  });

  it('renders loading indicator when loading', () => {
    (useMovieContext as jest.Mock).mockReturnValue({
      ...mockMovieContext,
      loading: true,
    });

    render(
      <BrowserRouter
        future={{
          v7_startTransition: true,
          v7_relativeSplatPath: true,
        }}
      >
        <ThemeProvider theme={lightTheme}>
          <SearchView />
        </ThemeProvider>
      </BrowserRouter>
    );

    expect(screen.getByText(/finding movies/i)).toBeInTheDocument();
  });

  it('renders error message when there is an error', () => {
    (useMovieContext as jest.Mock).mockReturnValue({
      ...mockMovieContext,
      error: 'Something went wrong',
    });

    render(
      <BrowserRouter
        future={{
          v7_startTransition: true,
          v7_relativeSplatPath: true,
        }}
      >
        <ThemeProvider theme={lightTheme}>
          <SearchView />
        </ThemeProvider>
      </BrowserRouter>
    );

    expect(screen.getByText(/something went wrong/i)).toBeInTheDocument();
  });
});
