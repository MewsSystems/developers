import { render, screen } from '@testing-library/react';
import { expect, it } from 'vitest';
import Footer from './Footer';

it('renders the footer', () => {
  render(<Footer />);

  const quickInfo = screen.getByTestId('movie-search-footer');
  expect(quickInfo).toBeInTheDocument();
});
