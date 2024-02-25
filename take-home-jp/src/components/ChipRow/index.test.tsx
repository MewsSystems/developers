import React from 'react';
import { render, screen } from '@testing-library/react';
import ChipRow from './';

test('renders button correctly', () => {
  const testCollection = [
    { id: 123, name: 'dog' },
    { id: 345, name: 'cat' },
  ];
  render(<ChipRow collection={testCollection} />);
  testCollection.map((item) => {
    const textElement = screen.getByText(item.name);
    expect(textElement).toBeInTheDocument();
  });
});
