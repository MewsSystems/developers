import React from 'react';
import { render, screen } from '@testing-library/react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import App from '../components/App';
import MainView from '../components/MainView';
import NotFoundPage from '../components/movieComponent/NotFound';

const mockStore = configureStore([]);

test('renders App component', () => {
  const initialState = {
    movies: {
      loading: false,
      movies: [],
      movieDetails: null,
      error: null,
      searchText: '',
      total_pages: 0,
    },
  };

  const store = mockStore(initialState);

  render(
    <Provider store={store}>
        <App />
    </Provider>
  );

  // Assert that MainView component is rendered
  expect(screen.getByText('Movie Search')).toBeInTheDocument();
  
});
