import { render, screen } from '@testing-library/react';
import { Details } from '../../models/Details';
import MovieQuickInfo from './MovieQuickInfo';

it('renders the movie quick info section', async () => {
  render(<MovieQuickInfo details={{} as Details} title="Movie Title" />);

  const quickInfo = screen.getByTestId('movie-quick-info-section');
  expect(quickInfo).toBeInTheDocument();
});
