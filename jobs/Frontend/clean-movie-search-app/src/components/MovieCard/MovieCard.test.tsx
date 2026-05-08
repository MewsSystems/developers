import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { ThemeProvider } from 'styled-components';
import { lightTheme } from '../../theme/themes';
import { MovieCard } from './MovieCard';
import { Movie } from '../../api';

describe('MovieCard', () => {
  const mockMovie: Movie = {
    id: 1,
    title: 'Movie Title',
    overview: 'Movie overview',
    poster_path: '/poster.jpg',
    release_date: '2024-11-17',
    vote_average: 7.8,
  };

  it('renders the movie card correctly', () => {
    render(
      <ThemeProvider theme={lightTheme}>
        <MovieCard movie={mockMovie} onClick={() => {}} />
      </ThemeProvider>
    );

    expect(screen.getByRole('img', { name: 'Movie Title' })).toHaveAttribute(
      'src',
      'https://image.tmdb.org/t/p/w500/poster.jpg'
    );
    expect(screen.getByText('Movie Title')).toBeInTheDocument();
    expect(screen.getByText('11/17/2024')).toBeInTheDocument();
    expect(screen.getByText('7.8')).toBeInTheDocument();
  });

  it('renders a placeholder image if poster_path is missing', () => {
    render(
      <ThemeProvider theme={lightTheme}>
        <MovieCard
          movie={{ ...mockMovie, poster_path: null }}
          onClick={() => {}}
        />
      </ThemeProvider>
    );

    expect(screen.getByRole('img', { name: 'Movie Title' })).toHaveAttribute(
      'src',
      'https://via.placeholder.com/500x750?text=Movie%20Title'
    );
  });

  it('renders the correct rating color based on vote_average', () => {
    render(
      <ThemeProvider theme={lightTheme}>
        <MovieCard movie={mockMovie} onClick={() => {}} />
      </ThemeProvider>
    );

    const ratingElement = screen.getByText('7.8');
    expect(ratingElement).toHaveStyle(
      `color: ${lightTheme.colors.rating.highRating.text}; border: 2px solid ${lightTheme.colors.rating.highRating.border}`
    );
  });

  it('matches the snapshot', () => {
    const { container } = render(
      <ThemeProvider theme={lightTheme}>
        <MovieCard movie={mockMovie} onClick={() => {}} />
      </ThemeProvider>
    );
    expect(container).toMatchSnapshot();
  });

  it('calls onClick with the correct movie ID when clicked', () => {
    const mockOnClick = jest.fn();
    render(
      <ThemeProvider theme={lightTheme}>
        <MovieCard movie={mockMovie} onClick={mockOnClick} />
      </ThemeProvider>
    );

    const cardElement = screen.getByRole('article');
    fireEvent.click(cardElement);

    expect(mockOnClick).toHaveBeenCalledWith(mockMovie.id);
  });
});
