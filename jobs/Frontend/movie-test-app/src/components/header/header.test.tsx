import { Header } from './';
import { render, screen } from '@testing-library/react';
import { test, describe, vi, expect } from 'vitest';
import { ThemeProvider } from 'styled-components';
import { defaultTheme } from '../../theme/theme.ts';
import { createMemoryRouter, RouterProvider } from 'react-router-dom';
import { MoviesRoute } from '../../app/routes/movies';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import * as useInfiniteMovies from '../../app/api/movies.ts';
import { cleanup } from '@testing-library/react';
import { MovieSearchResponse } from '../../types/api.ts';
import userEvent from '@testing-library/user-event';

Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: vi.fn().mockImplementation((query) => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: vi.fn(), // deprecated
    removeListener: vi.fn(), // deprecated
    addEventListener: vi.fn(),
    removeEventListener: vi.fn(),
    dispatchEvent: vi.fn(),
  })),
});

const queryClient = new QueryClient();

const fakeMovieSearchResponse: MovieSearchResponse = {
  page: 1,
  results: [
    {
      adult: false,
      backdrop_path: '',
      genre_ids: [],
      id: 1138148,
      original_language: 'pt',
      original_title: 'CIANETO',
      overview: '',
      popularity: 0.083,
      poster_path: '',
      release_date: '2023-05-03',
      title: 'CIANETO',
      video: false,
      vote_average: 0.0,
      vote_count: 0,
    },
  ],
  total_pages: 1,
  total_results: 3,
};

describe('Header', () => {
  const useInfiniteMoviesSpy = vi.spyOn(useInfiniteMovies, 'useInfiniteMovies');

  afterEach(() => {
    useInfiniteMoviesSpy.mockClear();
    cleanup();
  });

  test('test settings ', async () => {
    const routesConfig = [
      {
        path: '/movies',
        element: <MoviesRoute />,
      },
    ];
    const router = createMemoryRouter(routesConfig, {
      initialEntries: ['/movies'],
    });

    useInfiniteMoviesSpy.mockReturnValue({
      isLoading: false,
      isError: false,
      data: fakeMovieSearchResponse,
    });

    render(
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={defaultTheme}>
          <RouterProvider router={router}></RouterProvider>
        </ThemeProvider>
      </QueryClientProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    //const backButton = screen.getByTestId('back-button');
    //expect(backButton).toBeDefined();
    const settingsButton = screen.getByTestId('settings-button');
    expect(settingsButton).toBeDefined();
    await userEvent.click(settingsButton);
    const settingsContainer = screen.getByTestId('theme-select');
    expect(settingsContainer).toBeDefined();
  });
});
