import { render, screen } from '@testing-library/react';
import MovieDetails from './MovieDetails';

it('renders the movie details page', async () => {
  render(<MovieDetails />);

  const detailsPage = screen.getByTestId('movie-details-page');
  expect(detailsPage).toBeInTheDocument();
});
