import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import SearchContainer from './search-container';

describe('SearchContainer component', () => {
  it('renders with correct title and subtitle', () => {
    const { getByText } = render(
      <SearchContainer searchTerm="" handleOnSearch={() => {}} />,
    );

    expect(getByText('Discover Your Favorite Movies')).toBeInTheDocument();
    expect(
      getByText(
        'Search for any movie by title. Find detailed information for all your favorite films. Start exploring now!',
      ),
    ).toBeInTheDocument();
  });

  it('calls handleOnSearch when search is triggered', () => {
    const mockHandleOnSearch = jest.fn();
    const { getByPlaceholderText } = render(
      <SearchContainer searchTerm="" handleOnSearch={mockHandleOnSearch} />,
    );

    const searchInput = getByPlaceholderText('Search...');
    fireEvent.change(searchInput, { target: { value: 'Avatar' } });
    expect(mockHandleOnSearch).toHaveBeenCalledWith('Avatar');
  });
});
