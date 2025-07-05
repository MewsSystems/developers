import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { MovieListSkeleton } from './MovieListSkeleton';

describe('MovieListSkeleton', () => {
  it('renders the correct number of skeleton items', () => {
    render(<MovieListSkeleton itemNumber={5} />);

    expect(screen.getAllByTestId('movie-list-item-skeleton')).toHaveLength(5);
  });

  it('renders zero skeletons if itemNumber is 0', () => {
    render(<MovieListSkeleton itemNumber={0} />);

    expect(screen.queryAllByTestId('movie-list-item-skeleton')).toHaveLength(0);
  });
});
