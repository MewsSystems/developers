import { HttpResponse, http } from 'msw';
import { mockIntersectionObserver } from 'jsdom-testing-mocks';
import {
  act,
  fireEvent,
  renderWithQueryClientAndRouter,
  screen,
  waitFor,
  within,
} from '@/utils/test-utils';
import Search from '@/pages/search';
import { server } from '@/mocks/server';
import { routes } from '@/router';
import { moviesResultMock } from '@/mocks/data';

describe('Movies Search page', () => {
  beforeAll(() => {
    server.listen();
    vi.useFakeTimers({ shouldAdvanceTime: true });
  });
  afterEach(() => {
    server.resetHandlers();
    vi.runOnlyPendingTimers();
    vi.useRealTimers();
  });
  afterAll(() => server.close());

  const io = mockIntersectionObserver();

  test('should render the page, query movies and cache request', async () => {
    const movieSearchHandler = vi.fn(async ({ request }) => {
      const requestUrl = new URL(request.url);
      const page = Number(requestUrl.searchParams.get('page'));
      const currentPage = page - 1;
      return HttpResponse.json(moviesResultMock[currentPage % 2]);
    });

    server.use(
      http.get('https://api.themoviedb.org/3/search/movie', movieSearchHandler),
    );

    renderWithQueryClientAndRouter(<Search />, {
      routes,
    });

    screen.getByText('Explore movies');

    expect(screen.getByText('Explore movies')).toBeInTheDocument();
    expect(screen.getByText('Use the search bar above')).toBeInTheDocument();

    const search = screen.getByPlaceholderText('Search movies...');

    fireEvent.change(search, { target: { value: 'Batman' } });

    const moviesList = await screen.findByRole('list');
    const moviesItems = within(moviesList).getAllByRole('listitem');

    expect(moviesItems).toHaveLength(20);

    expect(movieSearchHandler).toHaveBeenCalledOnce();

    const numMoviesElement = screen.getByText('159').parentElement;
    expect(numMoviesElement?.textContent).toEqual('159 matches for Batman');

    const lastMovie = moviesItems[19];

    act(() => {
      // Simulate last movie appearing in the viewport
      io.enterNode(lastMovie);
    });

    // Should load 2nd page
    await waitFor(async () => {
      expect(screen.getByText('The Batman vs. Dracula')).toBeInTheDocument();

      const moviesList = await screen.findByRole('list');
      const moviesItems = await within(moviesList).findAllByRole('listitem');

      expect(movieSearchHandler).toHaveBeenCalledTimes(2);

      expect(moviesItems).toHaveLength(40);
    });

    const moviesList1 = await screen.findByRole('list');
    const moviesItems1 = await within(moviesList1).findAllByRole('listitem');
    const firstMovie = moviesItems1[0];

    fireEvent.click(firstMovie.children[0]);

    const movieOverview = await screen.findByText(
      "Andy moves to New York to work in the fashion industry. Her boss is extremely demanding, cruel and won't let her succeed if she doesn't fit into the high class elegant look of their magazine.",
    );

    expect(movieOverview).toBeInTheDocument();

    const homeLink = screen.getByLabelText('Home');

    fireEvent.click(homeLink);

    const moviesList2 = await screen.findByRole('list');
    const moviesItems2 = within(moviesList2).getAllByRole('listitem');

    // New request is not done, since it is cached for 30 minutes
    expect(movieSearchHandler).toHaveBeenCalledTimes(2);
    const firstMovie2 = moviesItems2[0];

    fireEvent.click(firstMovie2.children[0]);

    await screen.findByText(
      "Andy moves to New York to work in the fashion industry. Her boss is extremely demanding, cruel and won't let her succeed if she doesn't fit into the high class elegant look of their magazine.",
    );

    // Advance timers by 30 minutes
    const ONE_DAY = 1000 * 60 * 30;
    vi.advanceTimersByTime(ONE_DAY);

    const homeLink1 = screen.getByLabelText('Home');

    fireEvent.click(homeLink1);

    await screen.findByRole('list');

    // After 30 minutes the requests got stale,
    // so two more requests are done (page 1 and page 2)
    expect(movieSearchHandler).toHaveBeenCalledTimes(4);
  });
});
