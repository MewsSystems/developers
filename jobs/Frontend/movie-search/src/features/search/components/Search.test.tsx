import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Search from './Search';

it('renders search input and search grid on input', async () => {
  render(<Search />);

  const query = 'Star Wars';
  const searchInput: HTMLInputElement = screen.getByTestId('movie-search-input');

  expect(searchInput).toBeInTheDocument();

  await userEvent.type(searchInput, query);
  expect(searchInput).toHaveTextContent(query);

  const searchGrid = screen.getByTestId('movie-search-grid');
  expect(searchGrid).toBeInTheDocument();
});

it('allows clearing search input which sould remove the search grid from DOM', async () => {
  render(<Search />);

  const query = 'Lord of the Rings';
  const searchInput: HTMLInputElement = screen.getByTestId('movie-search-input');

  await userEvent.type(searchInput, query);
  expect(searchInput.value).toBe(query);
  const searchGrid = screen.getByTestId('movie-search-grid');

  await userEvent.clear(searchInput);
  expect(searchInput.value).toBe('');

  expect(searchGrid).not.toBeInTheDocument();
});
