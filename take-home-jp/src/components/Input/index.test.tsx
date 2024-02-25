import React from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import Input from './';

test('renders button correctly', () => {
  render(<Input onChange={jest.fn} />);
  const inputElement = screen.getByPlaceholderText('Search for a movie');
  expect(inputElement).toBeInTheDocument();
  fireEvent.change(inputElement, { target: { value: 'Godfather' } });
});
