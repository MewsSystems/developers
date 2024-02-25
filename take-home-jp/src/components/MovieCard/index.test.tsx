import React from 'react';
import { render, screen } from '@testing-library/react';
import MovieCard from './';
import { Movie } from './types';

const mockUsedNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockUsedNavigate,
}));

test('renders movie card correctly', () => {
  const testMovie: Movie = {
    id: 1,
    title: 'Spiderman',
    overview: 'Spiderman saves the day',
    poster_path: 'spiderman1.jpg',
    backdrop_path: 'spiderman2.jpg',
    favourite_count: 2,
    release_date: '1999-10-15',
    vote_average: 8.433,
    vote_count: 26279,
    genres: [],
  };
  render(<MovieCard movie={testMovie} />);

  const textElement = screen.getByText(testMovie.title);
  expect(textElement).toBeInTheDocument();

  const overview = screen.getByText(testMovie.overview, { exact: false });
  expect(overview).toBeInTheDocument();
});
