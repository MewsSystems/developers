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

const mockedUseSearchParams = useSearchParams as unknown as ReturnType<typeof vi.fn>;

describe('BackToSearchLink', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders link to "/" with default text if no search param', () => {
    mockedUseSearchParams.mockReturnValue({
      get: () => null,
    } as SearchParamsMock);

    render(<BackToSearchLink />);
    const link = screen.getByRole('link');
    expect(link).toHaveAttribute('href', '/');
    expect(link).toHaveTextContent('Search for more movies');
  });

  it('renders link with search param only', () => {
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => (key === 'search' ? 'batman' : null),
    } as SearchParamsMock);

    render(<BackToSearchLink />);
    const link = screen.getByRole('link');
    expect(link).toHaveAttribute('href', '/?search=batman');
    expect(link).toHaveTextContent('Go back to search');
  });

  it('renders link with search and page=1 (should not include page)', () => {
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => {
        if (key === 'search') return 'spiderman';
        if (key === 'page') return '1';
        return null;
      },
    } as SearchParamsMock);

    render(<BackToSearchLink />);
    const link = screen.getByRole('link');
    expect(link).toHaveAttribute('href', '/?search=spiderman');
    expect(link).toHaveTextContent('Go back to search');
  });

  it('renders link with search and page != 1 (should include page)', () => {
    mockedUseSearchParams.mockReturnValue({
      get: (key: string) => {
        if (key === 'search') return 'godzilla';
        if (key === 'page') return '3';
        return null;
      },
    } as SearchParamsMock);

    render(<BackToSearchLink />);
    const link = screen.getByRole('link');
    expect(link).toHaveAttribute('href', '/?search=godzilla&page=3');
    expect(link).toHaveTextContent('Go back to search');
  });

  it('has the correct styling and icon', () => {
    mockedUseSearchParams.mockReturnValue({
      get: () => null,
    } as SearchParamsMock);

    render(<BackToSearchLink />);
    const link = screen.getByRole('link');
    expect(link.className).toMatch(/bg-cyan-700/);
    const icon = link.querySelector('svg');
    expect(icon).toBeInTheDocument();
    expect(icon).toHaveAttribute('aria-hidden');
  });
});
