import React from 'react';
import { render } from '@testing-library/react';
import NotFound from './not-found';

describe('NotFound component', () => {
  it('renders with correct title and subtitle', () => {
    const { getByText } = render(<NotFound />);

    expect(getByText('404 - Not found')).toBeInTheDocument();
    expect(getByText('The page does not exists')).toBeInTheDocument();
  });
});
