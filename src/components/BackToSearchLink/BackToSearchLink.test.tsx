import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { BackToSearchLink } from './BackToSearchLink';

type SearchParamsMock = {
  get: (key: string) => string | null;
};

vi.mock('next/navigation', () => ({
  useSearchParams: vi.fn(),
}));

import { useSearchParams } from 'next/navigation';
import { MovieDetailResponse } from '@/types/api';

// Mock FaAngleLeft to avoid SVG noise (optional)
vi.mock('react-icons/fa6', () => ({
  FaAngleLeft: () => <svg data-testid="icon" aria-hidden />,
}));

const testMovie = {
  id: 88,
  original_title: 'Spider-Man: No Way Home',
  title: 'Spider-Man: No Way Home',
  overview: 'Some description',
  poster_path: '/poster.jpg',
} as unknown as MovieDetailResponse;

const mockedUseSearchParams = useSearchParams as unknown as ReturnType<typeof vi.fn>;

describe('BackToSearchLink', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders link to "/" with default text if no search param', () => {
    mockedUseSearchParams.mockReturnValue({
      get: () => null,
    } as SearchParamsMock);

    render(<BackToSearchLink movie={testMovie} />);

    const link = screen.getByRole('link');

    expect(link).toHaveAttribute('href', '/');
    expect(link).toHaveTextContent('Search for more movies');
    expect(link.querySelector('svg')).toBeInTheDocument();
    expect(link.querySelector('svg')).toHaveAttribute('aria-hidden');
  });

  it('renders link with search param only and correct hash', () => {
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => (key === 'search' ? 'batman' : null),
    } as SearchParamsMock);

    render(<BackToSearchLink movie={testMovie} />);
    const link = screen.getByRole('link');

    expect(link).toHaveAttribute('href', '/?search=batman#88-spider-man-no-way-home');
    expect(link).toHaveTextContent('Go back to search');
  });

  it('renders link with search and page=1 (should not include page), hash correct', () => {
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => {
        if (key === 'search') return 'spiderman';
        if (key === 'page') return '1';
        return null;
      },
    } as SearchParamsMock);

    render(<BackToSearchLink movie={testMovie} />);

    const link = screen.getByRole('link');

    expect(link).toHaveAttribute('href', '/?search=spiderman#88-spider-man-no-way-home');
    expect(link).toHaveTextContent('Go back to search');
  });

  it('renders link with search and page != 1 (should include page), hash correct', () => {
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => {
        if (key === 'search') return 'godzilla';
        if (key === 'page') return '3';
        return null;
      },
    } as SearchParamsMock);

    render(<BackToSearchLink movie={testMovie} />);

    const link = screen.getByRole('link');

    expect(link).toHaveAttribute('href', '/?search=godzilla&page=3#88-spider-man-no-way-home');
    expect(link).toHaveTextContent('Go back to search');
  });

  it('hash in link matches createMovieSlug for special titles', () => {
    const specialMovie = {
      ...testMovie,
      id: 99,
      original_title: 'Iron Man: The Rise!',
    };
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => (key === 'search' ? 'ironman' : null),
    } as SearchParamsMock);

    render(<BackToSearchLink movie={specialMovie} />);
    const link = screen.getByRole('link');

    expect(link.getAttribute('href')).toMatch(/#99-iron-man-the-rise$/);
  });

  it('has the correct styling', () => {
    mockedUseSearchParams.mockReturnValue({
      get: () => null,
    } as SearchParamsMock);

    render(<BackToSearchLink movie={testMovie} />);

    const link = screen.getByRole('link');

    expect(link.className).toMatch(/bg-cyan-700/);
  });
});
