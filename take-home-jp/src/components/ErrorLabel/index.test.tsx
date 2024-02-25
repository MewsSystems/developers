import React from 'react';
import { render, screen } from '@testing-library/react';
import ErrorLabel from './';

test('renders label correctly', () => {
  const text = 'api failed';
  render(<ErrorLabel errorMessage={text} />);
  const textElement = screen.getByText(text);
  expect(textElement).toBeInTheDocument();
});
