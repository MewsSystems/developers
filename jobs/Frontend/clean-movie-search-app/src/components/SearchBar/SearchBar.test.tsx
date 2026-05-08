import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { ThemeProvider } from 'styled-components';
import { lightTheme } from '../../theme/themes';
import { SearchBar } from './SearchBar';

describe('SearchBar', () => {
  it('updates the input value on change', () => {
    const mockOnChange = jest.fn();
    render(
      <ThemeProvider theme={lightTheme}>
        <SearchBar value="" onChange={mockOnChange} />
      </ThemeProvider>
    );

    const inputElement = screen.getByRole('textbox');
    fireEvent.change(inputElement, { target: { value: 'test' } });

    expect(mockOnChange).toHaveBeenCalledWith('test');
  });
});
