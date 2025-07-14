import { render, screen } from '@testing-library/react';
import { http, HttpResponse } from 'msw';
import { server } from '@/test/server';
import MoviePage from './page';
import type { MovieDetailResponse } from '@/types/api';

vi.mock('@/features/movies/MovieDetailsSection', () => ({
  MovieDetailsSection: ({
    movieData,
    error,
  }: {
    movieData: MovieDetailResponse | null;
    error?: string | null;
  }) => (
    <div data-testid="mock-movie-details-section">
      {movieData && <div data-testid="movie-id">{movieData.id}</div>}
      <div data-testid="movie-error">{error ?? 'none'}</div>
    </div>
  ),
}));

beforeAll(() => {
  process.env.NEXT_PUBLIC_SITE_URL = 'http://localhost:3000';
});
afterAll(() => {
  delete process.env.NEXT_PUBLIC_SITE_URL;
});

describe('<MoviePage />', () => {
  it('renders MovieDetailsSection with fetched movieData for valid slug', async () => {
    server.use(
      http.get('http://localhost:3000/api/movies/:id', ({ params }) => {
        expect(params.id).toBe('42');
        return HttpResponse.json({
          id: 42,
          title: 'Test Movie',
          overview: 'Just a test.',
          release_date: '2024-01-01',
          vote_average: 7,
          vote_count: 50,
          original_title: 'Test Movie',
          status: 'Released',
          runtime: 110,
          tagline: '',
          genres: [],
          spoken_languages: [],
          production_countries: [],
          origin_country: [],
          production_companies: [],
          poster_url: { default: null, sm: null, lg: null },
        } as MovieDetailResponse);
      })
    );

    const params = Promise.resolve({ movieId: '42-sluggy-movie' });
    const jsx = await MoviePage({ params });
    render(jsx);

    expect(await screen.findByTestId('mock-movie-details-section')).toBeInTheDocument();
    expect(screen.getByTestId('movie-id')).toHaveTextContent('42');
    expect(screen.getByTestId('movie-error')).toHaveTextContent('none');
  });

  it('renders MovieDetailsSection with error if fetch fails', async () => {
    server.use(
      http.get('http://localhost:3000/api/movies/:id', () => {
        return HttpResponse.error();
      })
    );

    const params = Promise.resolve({ movieId: '77-sluggy-movie' });
    const jsx = await MoviePage({ params });
    render(jsx);

    expect(await screen.findByTestId('mock-movie-details-section')).toBeInTheDocument();
    expect(screen.queryByTestId('movie-id')).not.toBeInTheDocument(); // No movieData
    expect(screen.getByTestId('movie-error')).toHaveTextContent(/Failed to fetch/i);
  });

  it('renders error section for invalid slug', async () => {
    const params = Promise.resolve({ movieId: 'not-a-valid-slug' });
    const jsx = await MoviePage({ params });
    render(jsx);

    expect(await screen.findByText(/Invalid movie slug format/i)).toBeInTheDocument();
  });
});
