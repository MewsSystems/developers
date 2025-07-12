import { render, screen } from '@testing-library/react';
import { MovieDetailsSection } from '@/features/movies/MovieDetailsSection';
import type { MovieDetailResponse } from '@/types/api';

vi.mock('@/components/BackToSearchLink', () => ({
  BackToSearchLink: () => <div>Mocked BackToSearchLink</div>,
}));

beforeAll(() => {
  process.env.NEXT_PUBLIC_SITE_URL = 'http://localhost:3000';
});

afterAll(() => {
  delete process.env.NEXT_PUBLIC_SITE_URL;
});

describe('<MovieDetailsSection />', () => {
  const mockMovie: MovieDetailResponse = {
    id: 1,
    title: 'Inception',
    overview: 'A skilled thief enters dreams to steal secrets.',
    release_date: '2010-07-16',
    vote_average: 8.8,
    vote_count: 1000,
    original_title: 'Inception',
    status: 'Released',
    runtime: 148,
    tagline: 'Your mind is the scene of the crime.',
    genres: [
      { id: 28, name: 'Action' },
      { id: 878, name: 'Sci-Fi' },
    ],
    spoken_languages: [
      {
        english_name: 'English',
        iso_639_1: 'en',
        name: 'English',
      },
    ],
    production_countries: [
      {
        iso_3166_1: 'US',
        name: 'United States of America',
      },
    ],
    origin_country: ['US'],
    production_companies: [
      {
        id: 1,
        name: 'Warner Bros.',
        logo_path: null,
        origin_country: 'US',
      },
    ],
    poster_url: {
      default: null,
      sm: null,
      lg: null,
    },
  };

  it('renders an error message if error is provided', () => {
    render(<MovieDetailsSection movieData={null} error="Something went wrong" />);
    expect(screen.getByText(/Error loading movie details/i)).toBeInTheDocument();
  });

  it('renders a fallback message if movieData is null', () => {
    render(<MovieDetailsSection movieData={null} />);
    expect(screen.getByText(/No movie data found/i)).toBeInTheDocument();
  });

  it('renders the title, meta, and movie content when movieData is provided', () => {
    render(<MovieDetailsSection movieData={mockMovie} />);

    expect(screen.getByText(/Mocked BackToSearchLink/i)).toBeInTheDocument();
    expect(screen.getByText(/Inception/i)).toBeInTheDocument();

    // Multiple copies due to responsive layout — don't use getByText
    expect(screen.getAllByText(/A skilled thief enters dreams to steal secrets/i)).not.toHaveLength(
      0
    );

    const title = document.querySelector('title');
    const meta = document.querySelector('meta[name="description"]');

    expect(title?.textContent).toBe('Search for movies: Inception');
    expect(meta?.getAttribute('content')).toBe(mockMovie.overview);
  });

  it('does not render a <meta> tag if no overview is present', () => {
    const movieWithoutOverview: MovieDetailResponse = {
      ...mockMovie,
      overview: '',
    };

    render(<MovieDetailsSection movieData={movieWithoutOverview} />);
    const meta = document.querySelector('meta[name="description"]');
    expect(meta).not.toBeInTheDocument();
  });
});
