import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import { useParams, useNavigate } from 'react-router-dom';
import { ThemeProvider } from 'styled-components';
import { lightTheme } from '../theme/themes';
import { MovieDetailsView } from './MovieDetailsView';
import { getMovieDetails } from '../api/movieApi';

// Mocking react-router-dom hooks
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn(),
  useNavigate: jest.fn(),
}));

// Mocking the getMovieDetails API call
jest.mock('../api/movieApi');

describe('MovieDetailsView', () => {
  const mockMovie = {
    id: 1,
    title: 'Movie Title',
    poster_path: '/poster.jpg',
    release_date: '2024-11-17',
    vote_average: 7.8,
    overview: 'Movie overview',
  };

  beforeEach(() => {
    (useParams as jest.Mock).mockReturnValue({ id: '1' });
    (useNavigate as jest.Mock).mockReturnValue(jest.fn());
  });

  it('fetches and displays movie details correctly', async () => {
    (getMovieDetails as jest.Mock).mockResolvedValue(mockMovie);

    render(
      <ThemeProvider theme={lightTheme}>
        <MovieDetailsView />
      </ThemeProvider>
    );

    // Check for loading state
    expect(screen.getByText(/loading movie details/i)).toBeInTheDocument();

    // Wait for the movie details to load
    await waitFor(() => {
      expect(screen.getByText('Movie Title')).toBeInTheDocument();
      expect(screen.getByText('Rating: 7.8')).toBeInTheDocument();
      expect(screen.getByText('Released: 11/17/2024')).toBeInTheDocument();
      expect(screen.getByText('Movie overview')).toBeInTheDocument();
    });
  });

  it('handles errors when fetching movie details', async () => {
    (getMovieDetails as jest.Mock).mockRejectedValue(
      new Error('Sorry, an error occurred while retrieving the movie details')
    );

    render(
      <ThemeProvider theme={lightTheme}>
        <MovieDetailsView />
      </ThemeProvider>
    );

    await waitFor(() => {
      expect(
        screen.getByText(
          /Sorry, an error occurred while retrieving the movie details/i
        )
      ).toBeInTheDocument();
    });
  });
});
