import React from 'react';
import { render, screen, within } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { MovieDetailsView } from './MovieDetailsView';
import type { MovieDetailResponse } from '@/types/api';

const exampleMovie: MovieDetailResponse = {
  id: 123,
  title: 'Test Movie',
  original_title: 'Original Test Movie',
  tagline: 'A tagline!',
  release_date: '2021-08-12',
  vote_average: 8.3,
  vote_count: 100,
  status: 'Coming soon',
  runtime: 142,
  spoken_languages: [{ english_name: 'English', iso_639_1: 'en', name: 'English' }],
  origin_country: ['US'],
  overview: 'This is an overview.',
  genres: [
    { id: 1, name: 'Drama' },
    { id: 2, name: 'Adventure' },
  ],
  production_companies: [
    { id: 1, name: 'OpenAI Productions', origin_country: 'US', logo_path: null },
    { id: 2, name: 'AI Films', origin_country: 'GB', logo_path: null },
  ],
  production_countries: [
    { iso_3166_1: 'US', name: 'United States' },
    { iso_3166_1: 'GB', name: 'United Kingdom' },
  ],
  poster_url: {
    default: '/storybook-assets/default-w185.webp',
    sm: '/storybook-assets/sm-w342.webp',
    lg: '/storybook-assets/lg-w500.webp',
  },
};

describe('MovieDetailsView', () => {
  it('renders all main movie info', () => {
    render(<MovieDetailsView movie={exampleMovie} />);

    expect(screen.getByRole('heading', { name: exampleMovie.title })).toBeInTheDocument();
    expect(screen.getByText(`AKA: ${exampleMovie.original_title}`)).toBeInTheDocument();
    expect(screen.getByText(/Tagline:/)).toBeInTheDocument();
    expect(screen.getByText(exampleMovie.tagline)).toBeInTheDocument();

    const integrated = screen.getByTestId('integrated-overview-content');
    const standalone = screen.getByTestId('standalone-overview-content');

    [integrated, standalone].forEach((container) => {
      const c = within(container);
      expect(c.getByText('Overview')).toBeInTheDocument();
      expect(c.getByText(exampleMovie.overview)).toBeInTheDocument();
      expect(c.getByText('Genres')).toBeInTheDocument();
      expect(c.getByText('Drama')).toBeInTheDocument();
      expect(c.getByText('Adventure')).toBeInTheDocument();
      expect(c.getByText('Production Companies')).toBeInTheDocument();
      expect(c.getByText('OpenAI Productions')).toBeInTheDocument();
      expect(c.getByText('AI Films')).toBeInTheDocument();
      expect(c.getByText('Production Countries')).toBeInTheDocument();
      expect(c.getByText('United States')).toBeInTheDocument();
      expect(c.getByText('United Kingdom')).toBeInTheDocument();
    });

    expect(screen.getByText('Languages:')).toBeInTheDocument();
    expect(screen.getByText('English')).toBeInTheDocument();
    expect(screen.getByText('Origin Countries:')).toBeInTheDocument();
    expect(screen.getByText('US')).toBeInTheDocument();
    expect(screen.getByAltText(/Poster for Test Movie/)).toBeInTheDocument();
    expect(screen.getByText(/Score:/)).toBeInTheDocument();
    expect(screen.getByText(/Released:/)).toBeInTheDocument();
    expect(screen.getByText(/12 Aug 2021/)).toBeInTheDocument();
    expect(screen.getByText(/Runtime:/)).toBeInTheDocument();
    expect(screen.getByText(/142 mins/)).toBeInTheDocument();
    expect(screen.getByText(/Status:/)).toBeInTheDocument();
    expect(screen.getByText(/Coming soon/)).toBeInTheDocument();
  });

  it('renders no original title and tagline if missing', () => {
    const movie = { ...exampleMovie, tagline: '', original_title: exampleMovie.title };

    render(<MovieDetailsView movie={movie} />);

    expect(screen.queryByText(/Tagline:/)).not.toBeInTheDocument();
    expect(screen.queryByText(/AKA:/)).not.toBeInTheDocument();
  });

  it('does NOT render genres if empty', () => {
    const movie = { ...exampleMovie, genres: [] };

    render(<MovieDetailsView movie={movie} />);

    const integrated = screen.getByTestId('integrated-overview-content');
    const standalone = screen.getByTestId('standalone-overview-content');

    [integrated, standalone].forEach((container) => {
      const c = within(container);
      expect(c.queryByText('Genres')).not.toBeInTheDocument();
      expect(c.queryByText('Drama')).not.toBeInTheDocument();
      expect(c.queryByText('Adventure')).not.toBeInTheDocument();
    });
  });

  it('does NOT render production companies if empty', () => {
    const movie = { ...exampleMovie, production_companies: [] };

    render(<MovieDetailsView movie={movie} />);

    const integrated = screen.getByTestId('integrated-overview-content');
    const standalone = screen.getByTestId('standalone-overview-content');

    [integrated, standalone].forEach((container) => {
      const c = within(container);
      expect(c.queryByText('Production Companies')).not.toBeInTheDocument();
      expect(c.queryByText('OpenAI Productions')).not.toBeInTheDocument();
      expect(c.queryByText('AI Films')).not.toBeInTheDocument();
    });
  });

  it('does NOT render production countries if empty', () => {
    const movie = { ...exampleMovie, production_countries: [] };

    render(<MovieDetailsView movie={movie} />);

    const integrated = screen.getByTestId('integrated-overview-content');
    const standalone = screen.getByTestId('standalone-overview-content');

    [integrated, standalone].forEach((container) => {
      const c = within(container);
      expect(c.queryByText('Production Countries')).not.toBeInTheDocument();
      expect(c.queryByText('United States')).not.toBeInTheDocument();
      expect(c.queryByText('United Kingdom')).not.toBeInTheDocument();
    });
  });

  it('renders nothing from OverviewContent if all fields are empty', () => {
    const movie = {
      ...exampleMovie,
      overview: '',
      genres: [],
      production_companies: [],
      production_countries: [],
    };

    render(<MovieDetailsView movie={movie} />);

    const integrated = screen.getByTestId('integrated-overview-content');
    const standalone = screen.getByTestId('standalone-overview-content');

    [integrated, standalone].forEach((container) => {
      const c = within(container);
      expect(c.queryByText('Overview')).not.toBeInTheDocument();
      expect(c.queryByText('Genres')).not.toBeInTheDocument();
      expect(c.queryByText('Production Companies')).not.toBeInTheDocument();
      expect(c.queryByText('Production Countries')).not.toBeInTheDocument();
    });
  });

  it('renders language and origin country chips if present', () => {
    render(<MovieDetailsView movie={exampleMovie} />);

    expect(screen.getByText('English')).toBeInTheDocument();
    expect(screen.getByText('US')).toBeInTheDocument();
  });

  it('renders Score as percentage', () => {
    render(<MovieDetailsView movie={exampleMovie} />);

    expect(screen.getByText('83%')).toBeInTheDocument();
  });

  it('shows "unknown" for missing runtime', () => {
    const movie = { ...exampleMovie, runtime: 0 };

    render(<MovieDetailsView movie={movie} />);

    expect(screen.getByText(/unknown/)).toBeInTheDocument();
  });

  it('shows "no votes" if vote_average is not finite', () => {
    const movie = { ...exampleMovie, vote_average: NaN, vote_count: 0 };

    render(<MovieDetailsView movie={movie} />);

    expect(screen.getByText('no votes')).toBeInTheDocument();
  });

  it('renders ReleaseDate as formatted date', () => {
    const movie = { ...exampleMovie, release_date: '' };

    render(<MovieDetailsView movie={movie} />);

    expect(screen.getByText(/unknown/)).toBeInTheDocument();
  });
});
