import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';

import posterNotFound from '@/assets/posterNotFound.svg';
import { MOCK_MOVIE } from '@/const/test/testConst';

import { MovieCard } from './MovieCard';

const MOCK_MOVIE_NO_POSTER_PATH = {
  ...MOCK_MOVIE,
  poster_path: null,
};

describe('MovieCard', () => {
  it('renders with a movie poster when movie.poster_path is provided', () => {
    render(<MovieCard movie={MOCK_MOVIE} />);

    const image = screen.getByRole('img', { name: /movie poster/i });

    expect(image).toHaveAttribute(
      'src',
      expect.stringContaining(MOCK_MOVIE.poster_path)
    );
  });

  it('renders with a default poster when poster_path is not provided', () => {
    render(<MovieCard movie={MOCK_MOVIE_NO_POSTER_PATH} />);

    const image = screen.getByRole('img', { name: /movie poster/i });
    expect(image).toHaveAttribute('src', posterNotFound);
  });
});
