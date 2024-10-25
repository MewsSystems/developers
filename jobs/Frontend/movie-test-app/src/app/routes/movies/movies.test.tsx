import { render, screen } from '@testing-library/react';
import { test, describe, vi, expect } from 'vitest';
import { ThemeProvider } from 'styled-components';
import { defaultTheme } from '../../../theme/theme.ts';
import { createMemoryRouter, RouterProvider } from 'react-router-dom';
import { MoviesRoute } from './movies.route.tsx';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import * as useInfiniteMovies from '../../../app/api/movies.ts';
import { cleanup } from '@testing-library/react';
import cianeMovieSearchResponse from './ciane_infinite_query.json';

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

class MockIntersectionObserver implements IntersectionObserver {
  root: Document | Element | null = null;
  rootMargin: string = ``;
  thresholds: readonly number[] = [];

  disconnect = vi.fn();
  observe = vi.fn();
  takeRecords = vi.fn();
  unobserve = vi.fn();
}

const defaultBoundary = {
  x: 0,
  y: 0,
  width: 0,
  height: 0,
  top: 0,
  right: 0,
  bottom: 0,
  left: 0,
  toJSON: vi.fn(),
};

window.IntersectionObserver = MockIntersectionObserver;
const queryClient = new QueryClient();

describe('Movie Search', () => {
  const useInfiniteMoviesSpy = vi.spyOn(useInfiniteMovies, 'useInfiniteMovies');

  afterEach(() => {
    useInfiniteMoviesSpy.mockClear();
    cleanup();
  });

  test('Test initial loading without search', async () => {
    const routesConfig = [
      {
        path: '/movies',
        element: <MoviesRoute />,
      },
    ];
    const router = createMemoryRouter(routesConfig, {
      initialEntries: ['/movies'],
    });

    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-expect-error
    useInfiniteMoviesSpy.mockReturnValue({
      data: undefined,
      isLoading: false,
      isError: false,
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
    const moviesContainer = screen.getByTestId('movies-container');
    expect(moviesContainer).toBeDefined();
    expect(useInfiniteMoviesSpy).toHaveBeenCalled();
    const dividers = screen.queryAllByTestId(/movie-card-/i);
    expect(dividers.length).toBe(0);
  });

  test('Test loading with search "ciane" in desktop mode', async () => {
    const routesConfig = [
      {
        path: '/movies',
        element: <MoviesRoute />,
      },
    ];
    const router = createMemoryRouter(routesConfig, {
      initialEntries: ['/movies'],
    });

    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-expect-error
    useInfiniteMoviesSpy.mockReturnValue(cianeMovieSearchResponse);

    render(
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={defaultTheme}>
          <RouterProvider router={router}></RouterProvider>
        </ThemeProvider>
      </QueryClientProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const moviesContainer = screen.getByTestId('movies-container');
    expect(moviesContainer).toBeDefined();
    expect(useInfiniteMoviesSpy).toHaveBeenCalled();
    const dividers = screen.queryAllByTestId(/movie-card-/i);
    expect(dividers.length).toBe(3);
    const firstMovie = dividers[0];
    expect(firstMovie).toBeDefined();
    firstMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary }));
    const secondMovie = dividers[1];
    expect(secondMovie).toBeDefined();
    secondMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary, left: 1 }));
    const thirdMovie = dividers[2];
    expect(thirdMovie).toBeDefined();
    secondMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary, left: 2 }));

    //expect all the movies to be on the same row because we are desktop mode
    expect(firstMovie.getBoundingClientRect().top).toBe(secondMovie.getBoundingClientRect().top);
    expect(secondMovie.getBoundingClientRect().top).toBe(thirdMovie.getBoundingClientRect().top);
    expect(firstMovie.getBoundingClientRect().left).not.toBe(secondMovie.getBoundingClientRect().left);
    expect(secondMovie.getBoundingClientRect().left).not.toBe(thirdMovie.getBoundingClientRect().left);
  });

  test('Test loading with search "ciane" in mobile mode', async () => {
    const routesConfig = [
      {
        path: '/movies',
        element: <MoviesRoute />,
      },
    ];
    const router = createMemoryRouter(routesConfig, {
      initialEntries: ['/movies'],
    });

    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-expect-error
    useInfiniteMoviesSpy.mockReturnValue(cianeMovieSearchResponse);

    vi.spyOn(window.screen, 'width', 'get').mockReturnValue(300);

    render(
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={defaultTheme}>
          <RouterProvider router={router}></RouterProvider>
        </ThemeProvider>
      </QueryClientProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const moviesContainer = screen.getByTestId('movies-container');
    expect(moviesContainer).toBeDefined();
    expect(useInfiniteMoviesSpy).toHaveBeenCalled();
    const dividers = screen.queryAllByTestId(/movie-card-/i);
    expect(dividers.length).toBe(3);
    const firstMovie = dividers[0];
    expect(firstMovie).toBeDefined();
    firstMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary }));
    const secondMovie = dividers[1];
    expect(secondMovie).toBeDefined();
    secondMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary, top: 1 }));
    const thirdMovie = dividers[2];
    expect(thirdMovie).toBeDefined();
    thirdMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary, top: 2 }));

    //expect all the movies to be on the same column because we are mobile mode
    expect(firstMovie.getBoundingClientRect().left).toBe(secondMovie.getBoundingClientRect().left);
    expect(secondMovie.getBoundingClientRect().left).toBe(thirdMovie.getBoundingClientRect().left);
    expect(firstMovie.getBoundingClientRect().top).not.toBe(secondMovie.getBoundingClientRect().top);
    expect(secondMovie.getBoundingClientRect().top).not.toBe(thirdMovie.getBoundingClientRect().top);
  });

  test('Test loading with search "ciane" in tablet mode', async () => {
    const routesConfig = [
      {
        path: '/movies',
        element: <MoviesRoute />,
      },
    ];
    const router = createMemoryRouter(routesConfig, {
      initialEntries: ['/movies'],
    });

    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-expect-error
    useInfiniteMoviesSpy.mockReturnValue(cianeMovieSearchResponse);

    vi.spyOn(window.screen, 'width', 'get').mockReturnValue(700);

    render(
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={defaultTheme}>
          <RouterProvider router={router}></RouterProvider>
        </ThemeProvider>
      </QueryClientProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const moviesContainer = screen.getByTestId('movies-container');
    expect(moviesContainer).toBeDefined();
    expect(useInfiniteMoviesSpy).toHaveBeenCalled();
    const dividers = screen.queryAllByTestId(/movie-card-/i);
    expect(dividers.length).toBe(3);
    const firstMovie = dividers[0];
    expect(firstMovie).toBeDefined();
    firstMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary }));
    const secondMovie = dividers[1];
    expect(secondMovie).toBeDefined();
    secondMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary, left: 1 }));
    const thirdMovie = dividers[2];
    expect(thirdMovie).toBeDefined();
    thirdMovie.getBoundingClientRect = vi.fn(() => ({ ...defaultBoundary, top: 1 }));

    //CARDS wiill be disposed as follwing:
    //
    // 1 2
    // 3

    // expect first and third movie to be on the same column
    // while the second movie is on the next column
    expect(firstMovie.getBoundingClientRect().left).toBe(thirdMovie.getBoundingClientRect().left);
    expect(secondMovie.getBoundingClientRect().left).not.toBe(firstMovie.getBoundingClientRect().left);

    //also second movie and first movie are on the first row
    //while the third movie is on the second row
    expect(firstMovie.getBoundingClientRect().top).toBe(secondMovie.getBoundingClientRect().top);
    expect(secondMovie.getBoundingClientRect().top).toBeLessThan(thirdMovie.getBoundingClientRect().top);
  });
});
