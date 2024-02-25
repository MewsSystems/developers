import React from 'react';
import { render, screen } from '@testing-library/react';
import Button from './';

test('renders button correctly', () => {
  const text = 'PRESS';
  render(<Button onClick={jest.fn()}>{text}</Button>);
  const textElement = screen.getByText(text);
  expect(textElement).toBeInTheDocument();
});
