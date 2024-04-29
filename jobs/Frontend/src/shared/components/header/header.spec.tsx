import React from 'react';
import { render } from '@testing-library/react';
import Header from './Header';

describe('Header Component', () => {
  test('renders header with logo', () => {
    const { getByAltText } = render(<Header />);
    const logoElement = getByAltText('Movie Search Logo');
    expect(logoElement).toBeInTheDocument();
  });
});
