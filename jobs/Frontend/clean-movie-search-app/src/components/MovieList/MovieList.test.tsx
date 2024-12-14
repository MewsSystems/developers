import React from 'react';
import { render, screen } from '@testing-library/react';
import { ThemeProvider } from 'styled-components';
import { lightTheme } from '../../theme/themes';
import { MovieList } from './MovieList';
import { Movie } from '../../api';

describe('MovieList', () => {
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

  it('renders the movie list correctly', () => {
    render(
      <ThemeProvider theme={lightTheme}>
        <MovieList movies={mockMovies} onMovieClick={() => {}} />
      </ThemeProvider>
    );

    expect(screen.getByText('Movie 1')).toBeInTheDocument();
    expect(screen.getByText('Movie 2')).toBeInTheDocument();
  });

  it('renders nothing when the movie list is empty', () => {
    render(
      <ThemeProvider theme={lightTheme}>
        <MovieList movies={[]} onMovieClick={() => {}} />
      </ThemeProvider>
    );

    // Assert that no movie titles are rendered
    expect(screen.queryByText('Movie 1')).not.toBeInTheDocument();
    expect(screen.queryByText('Movie 2')).not.toBeInTheDocument();
  });
});
