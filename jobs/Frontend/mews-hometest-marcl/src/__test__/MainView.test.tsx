import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import MainView from '../components/MainView';
import { searchMovies } from '../store/actions/movies.actions';

const mockStore = configureStore([]);

describe('MainView Component', () => {
  let store: any;
  beforeEach(() => {
    store = mockStore({
      movies: {
        loading: false,
        movies: [],
        movieDetails: null,
        error: null,
        searchText: '',
        total_pages: 0,
      },
    });
  });

  test('renders MainView component', () => {
    render(
      <Provider store={store}>
        <MainView />
      </Provider>
    );

    expect(screen.getByText('Movie Search')).toBeInTheDocument();
    expect(screen.queryByText('No movies found')).toBeNull();
    expect(screen.queryByTestId('spinner')).toBeNull();
    expect(screen.queryByTestId('error')).toBeNull();
  });

  test('displays error message when there is an error', () => {
    store = mockStore({
      movies: {
        loading: false,
        movies: [],
        movieDetails: null,
        error: {
          title: 'Error',
          details: 'An error occurred while searching.',
        },
        searchText: '',
        total_pages: 0,
      },
    });

    render(
      <Provider store={store}>
        <MainView />
      </Provider>
    );

    expect(screen.getByText('An error occurred while searching.')).toBeInTheDocument();
  });

});