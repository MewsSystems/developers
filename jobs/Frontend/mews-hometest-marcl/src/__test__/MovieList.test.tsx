import React from 'react';
import { render } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import MovieList from '../components/movieComponent/MovieList';
import { Movie } from '../types/movieTypes';

const mockStore = configureStore([]);
const store = mockStore({
  movies: { favorites: [] }, // Mocking Redux state
});

describe('MovieList Component', () => {
  test('renders movie list with movies', () => {
    const movies: Movie[] = [
      {
        id: 1,
        title: 'Movie 1',
        overview: 'Overview 1',
        release_date: '2022-03-19',
        poster_path: '/path/to/image1.jpg',
        vote_average: 7.5,
        runtime: 120,
        genres: [
          { id: 1, name: 'Action' },
          { id: 2, name: 'Adventure' },
        ],
      },
      {
        id: 2,
        title: 'Movie 2',
        overview: 'Overview 2',
        release_date: '2022-03-20',
        poster_path: '/path/to/image2.jpg',
        vote_average: 8.0,
        runtime: 130,
        genres: [
          { id: 3, name: 'Drama' },
          { id: 4, name: 'Thriller' },
        ],
      },
    ];

    const { getByText, getByAltText } = render(
      <Provider store={store}>
        <Router>
          <MovieList movies={movies} searchPerformed={false} />
        </Router>
      </Provider>
    );

    expect(getByText('Movie 1')).toBeInTheDocument();
    expect(getByText('Movie 2')).toBeInTheDocument();
    expect(getByText('Overview 1')).toBeInTheDocument();
    expect(getByText('Overview 2')).toBeInTheDocument();
    expect(getByAltText('Movie 1')).toBeInTheDocument();
    expect(getByAltText('Movie 2')).toBeInTheDocument();
  });

  test('renders "No movies found" when search is performed but no movies are found', () => {
    const { getByText } = render(
      <Provider store={store}>
        <Router>
          <MovieList movies={[]} searchPerformed={true} />
        </Router>
      </Provider>
    );

    const noMoviesElement = getByText('No movies found');
    expect(noMoviesElement).toBeInTheDocument();
  });
});
