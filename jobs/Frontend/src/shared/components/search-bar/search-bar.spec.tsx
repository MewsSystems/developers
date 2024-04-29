import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import SearchBar from './search-bar';

describe('SearchBar component', () => {
  it('renders input element', () => {
    const searchTerm = '';
    const onSearch = jest.fn();

    const { getByRole } = render(
      <SearchBar searchTerm={searchTerm} onSearch={onSearch} />,
    );
    expect(getByRole('textbox')).toBeInTheDocument();
  });

  it('calls onSearch callback with the correct search term', () => {
    const searchTerm = '';
    const onSearch = jest.fn();
    const newSearchTerm = 'movie';

    const { getByRole } = render(
      <SearchBar searchTerm={searchTerm} onSearch={onSearch} />,
    );

    fireEvent.change(getByRole('textbox'), {
      target: { value: newSearchTerm },
    });

    expect(onSearch).toHaveBeenCalledWith(newSearchTerm);
  });
});
