import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import MovieDetails from './MovieDetails';

const queryClient = new QueryClient();

const MockMovieDetails = () => (
  <QueryClientProvider client={queryClient}>
    <BrowserRouter>
      <MovieDetails />
    </BrowserRouter>
  </QueryClientProvider>
);

it('renders the movie details page', () => {
  render(<MockMovieDetails />);

  const detailsPage = screen.getByTestId('movie-details-page');
  expect(detailsPage).toBeInTheDocument();
});
