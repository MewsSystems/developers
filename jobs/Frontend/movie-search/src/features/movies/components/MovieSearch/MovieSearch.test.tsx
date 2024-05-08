import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { BrowserRouter } from 'react-router-dom';
import { describe, expect, it } from 'vitest';
import MovieSearch from './MovieSearch';

const queryClient = new QueryClient();

const MockMovieSearch = () => (
  <QueryClientProvider client={queryClient}>
    <BrowserRouter>
      <MovieSearch />
    </BrowserRouter>
  </QueryClientProvider>
);

describe('renders the search input', () => {
  render(<MockMovieSearch />);

  const query = 'The Lord of the Rings';
  const searchInput: HTMLInputElement = screen.getByRole('textbox');

  it('accepts and deletes text input correctly', async () => {
    expect(searchInput).toBeInTheDocument();

    await userEvent.type(searchInput, query);
    expect(searchInput.value).toBe(query);

    await userEvent.clear(searchInput);
    expect(searchInput.value).toBe('');
  });
});
