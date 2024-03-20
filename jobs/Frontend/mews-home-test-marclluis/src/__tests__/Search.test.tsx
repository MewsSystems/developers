import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import Search from '../components/movieComponent/Search';

const mockStore = configureStore([]);

describe('Search Component', () => {
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

  test('renders search input', () => {
    const { getByLabelText } = render(
      <Provider store={store}>
        <Search onSearch={() => {}}/>
      </Provider>
    );
    const searchInput = getByLabelText('Search movies') as HTMLInputElement;
    expect(searchInput).toBeInTheDocument();
    expect(searchInput.value).toBe('');
  });

  test('calls onSearch prop with search text on input change', () => {
    const mockOnSearch = jest.fn();
    const { getByLabelText } = render(
      <Provider store={store}>
        <Search onSearch={mockOnSearch} />
      </Provider>
    );
    const searchInput = getByLabelText('Search movies') as HTMLInputElement;

    fireEvent.change(searchInput, { target: { value: 'Avengers' } });

    expect(mockOnSearch).toHaveBeenCalledWith('Avengers');
  });
});
