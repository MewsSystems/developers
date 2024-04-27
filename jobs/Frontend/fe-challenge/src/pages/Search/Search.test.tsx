import { Outlet } from 'react-router-dom';
import { mockIntersectionObserver } from 'jsdom-testing-mocks';
import {
  act,
  fireEvent,
  renderWithQueryClientAndRouter,
  screen,
  waitFor,
  within,
} from '@/utils/test-utils';
import Search from '@/pages/Search';
import { server } from '@/mocks/server';
import Layout from '@/components/Layout';
import ErrorPage from '@/pages/ErrorPage';

describe('Movies Search page', () => {
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  const io = mockIntersectionObserver();

  test('should render the page and query movies', async () => {
    renderWithQueryClientAndRouter(<Search />, {
      routes: [
        {
          path: '/',
          element: <Layout />,
          children: [
            {
              element: <Outlet />,
              errorElement: <ErrorPage />,
              children: [{ index: true, element: <Search /> }],
            },
          ],
        },
      ],
    });

    screen.getByText('Explore movies');

    expect(screen.getByText('Explore movies')).toBeInTheDocument();
    expect(screen.getByText('Use the search bar above')).toBeInTheDocument();

    const search = screen.getByPlaceholderText('Search movies...');

    fireEvent.change(search, { target: { value: 'Batman' } });

    const moviesList = await screen.findByRole('list');
    const moviesItems = within(moviesList).getAllByRole('listitem');

    expect(moviesItems).toHaveLength(20);

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

      expect(moviesItems).toHaveLength(40);
    });
  });
});
