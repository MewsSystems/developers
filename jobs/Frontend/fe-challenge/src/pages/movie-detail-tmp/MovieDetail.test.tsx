import { renderWithQueryClientAndRouter, screen } from '@/utils/test-utils';
import { server } from '@/mocks/server';
import { routes } from '@/router';
import MovieDetail from '@/pages/movie-detail-tmp/MovieDetail';

describe('Movies detail page', () => {
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  test('should render the page and query movie detail', async () => {
    renderWithQueryClientAndRouter(<MovieDetail />, {
      routes,
      routerHistory: ['/movie/350'],
    });

    const loading = screen.getByText('Loading...');

    expect(loading).toBeInTheDocument();

    const movieOverview = await screen.findByText(
      "Andy moves to New York to work in the fashion industry. Her boss is extremely demanding, cruel and won't let her succeed if she doesn't fit into the high class elegant look of their magazine.",
    );

    expect(movieOverview).toBeInTheDocument();

    const tagline = screen.getByText(
      "Meet Andy Sachs. A million girls would kill to have her job. She's not one of them.",
    );

    expect(tagline).toBeInTheDocument();

    const posterAlt = screen.getByAltText('The Devil Wears Prada');

    expect(posterAlt).toBeInTheDocument();
  });
});
