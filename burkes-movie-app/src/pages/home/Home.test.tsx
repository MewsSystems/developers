import { UseQueryResult } from '@tanstack/react-query';
import { fireEvent, render, screen } from '@testing-library/react';
import { describe, expect, it, vi } from 'vitest';

import * as moviesQueries from '@/queries/moviesQueries';
import { customRender } from '@/test/utils/customRender';
import { MoviesApiResponse } from '@/types';

import { Home } from './Home';

describe('Home', () => {
  const mockUseMoviesSearchQuery = vi.spyOn(
    moviesQueries,
    'useMoviesSearchQuery'
  );

  it('displays prompt to search when the search bar is empty', () => {
    // Burkes - casting only for this mock impementation. Would not use outside of a test like this
    mockUseMoviesSearchQuery.mockReturnValue({
      data: undefined,
      isLoading: false,
    } as UseQueryResult<MoviesApiResponse, Error>);

    customRender(<Home />);

    expect(
      screen.getByText(/Please type something to begin search/i)
    ).toBeInTheDocument();
  });

  it('displays loading when fetching data', async () => {
    mockUseMoviesSearchQuery.mockReturnValue({
      data: undefined,
      isLoading: true,
    } as UseQueryResult<MoviesApiResponse, Error>);

    customRender(<Home />);
    fireEvent.change(screen.getByRole('textbox'), { target: { value: 'a' } });

    // await execution of debouncedSearch, which has 400ms delay
    await new Promise((resolve) => setTimeout(resolve, 500));

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('displays no results message whe search returns no results', async () => {
    mockUseMoviesSearchQuery.mockReturnValue({
      data: { results: [], total_pages: 0, page: 0, total_results: 0 },
      isLoading: false,
    } as UseQueryResult<MoviesApiResponse, Error>);

    render(<Home />);

    fireEvent.change(screen.getByRole('textbox'), { target: { value: 'a' } });

    // await execution of debouncedSearch, which has 400ms delay
    await new Promise((resolve) => setTimeout(resolve, 500));

    expect(
      screen.getByText(/no results found, please alter your search/i)
    ).toBeInTheDocument();
  });
});
