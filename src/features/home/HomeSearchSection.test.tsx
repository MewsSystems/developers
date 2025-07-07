import { render, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { HomeSearchSection } from './HomeSearchSection';
import { QueryClientProvider } from '@tanstack/react-query';
import { http, HttpResponse } from 'msw';
import { server } from '@/test/server';
import { createTestQueryClient } from '@/test/utils';

let originalScrollTo: typeof window.scrollTo;

beforeAll(() => {
  originalScrollTo = window.scrollTo;
  window.scrollTo = vi.fn();
});
afterAll(() => {
  window.scrollTo = originalScrollTo;
});

function renderWithClient(initialSearch = '', initialPage = 1) {
  const queryClient = createTestQueryClient();
  render(
    <QueryClientProvider client={queryClient}>
      <HomeSearchSection initialSearch={initialSearch} initialPage={initialPage} />
    </QueryClientProvider>
  );
}

function setupPaginatedMock(totalPages: number) {
  server.use(
    http.get('/api/movies', ({ request }) => {
      const url = new URL(request.url);
      const page = url.searchParams.get('page') ?? '1';

      return HttpResponse.json({
        results: [
          {
            id: parseInt(page),
            title: `Movie Page ${page}`,
            original_title: `Movie Page ${page}`,
            release_date: '2000-01-01',
            vote_average: 8,
            overview: `Page ${page} content`,
            poster_url: { default: null },
          },
        ],
        total_pages: totalPages,
      });
    })
  );
}

async function triggerSearch(query: string) {
  const input = screen.getByPlaceholderText(/Search movies.../i);
  await userEvent.clear(input);
  await userEvent.type(input, query);
  await waitFor(() => {
    expect(screen.getByText(`Movie Page 1`)).toBeInTheDocument();
  });
}

function getBottomPagination() {
  const paginations = screen.getAllByRole('navigation', { name: 'Pagination' });
  return paginations.find((p) => p.getAttribute('data-testid') !== 'pagination-top');
}

describe('HomeSearchSection', () => {
  it('renders welcome message and search input', () => {
    renderWithClient();

    expect(screen.getByText(/Welcome to MovieSearch/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Search movies.../i)).toBeInTheDocument();
  });

  it('shows loading and displays movie results', async () => {
    server.use(
      http.get('/api/movies', () =>
        HttpResponse.json({
          results: [
            {
              id: 1,
              title: 'The Matrix',
              original_title: 'The Matrix',
              release_date: '1999-03-31',
              vote_average: 8.7,
              overview: 'A hacker learns the truth...',
              poster_url: { default: null },
            },
          ],
          total_pages: 1,
        })
      )
    );

    renderWithClient();

    const input = screen.getByPlaceholderText(/Search movies.../i);
    await userEvent.clear(input);
    await userEvent.type(input, 'matrix');

    expect(await screen.findByRole('status')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.getByText(/The Matrix/i)).toBeInTheDocument();
    });
  });

  it('shows message when no results are found', async () => {
    server.use(http.get('/api/movies', () => HttpResponse.json({ results: [], total_pages: 1 })));

    renderWithClient();

    const input = screen.getByPlaceholderText(/Search movies.../i);
    await userEvent.clear(input);
    await userEvent.type(input, 'nothing');

    await waitFor(() => {
      expect(screen.getByText(/No results match your search/i)).toBeInTheDocument();
    });
  });

  it('shows error message when fetch fails', async () => {
    server.use(
      http.get('/api/movies', ({ request }) => {
        const url = new URL(request.url);
        const search = url.searchParams.get('search');

        if (search === 'fail') {
          return new HttpResponse(null, { status: 500 });
        }

        return HttpResponse.json({ results: [], total_pages: 1 });
      })
    );

    renderWithClient();

    const input = screen.getByPlaceholderText(/Search movies.../i);
    await userEvent.clear(input);
    await userEvent.type(input, 'fail');

    await waitFor(() => {
      expect(screen.getByRole('alert')).toHaveTextContent(
        /There was a problem fetching your search results/i
      );
    });
  });

  it('navigates to the next page, scrolls and focuses summary region', async () => {
    setupPaginatedMock(3);
    renderWithClient();

    await triggerSearch('test');

    const bottomPagination = getBottomPagination();
    const nextButton = within(bottomPagination!).getByRole('link', { name: /next page/i });
    await userEvent.click(nextButton);

    await waitFor(() => {
      expect(screen.getByText(/Movie Page 2/)).toBeInTheDocument();
    });

    await waitFor(() => {
      const summaryRegion = screen.getByTestId('results-summary-container');
      expect(document.activeElement).toBe(summaryRegion);
    });

    expect(window.scrollTo).toHaveBeenCalledWith({ top: 0 });
  });

  it('navigates to the previous page using the pagination control', async () => {
    setupPaginatedMock(3);
    renderWithClient();

    await triggerSearch('test');

    const bottomPagination = getBottomPagination();
    const nextButton = within(bottomPagination!).getByRole('link', { name: /next page/i });
    await userEvent.click(nextButton);

    await waitFor(() => {
      expect(screen.getByText(/Movie Page 2/)).toBeInTheDocument();
    });

    const prevButton = within(bottomPagination!).getByRole('link', { name: /previous page/i });
    await userEvent.click(prevButton);

    await waitFor(() => {
      expect(screen.getByText(/Movie Page 1/)).toBeInTheDocument();
    });
  });

  it('navigates to a specific page using the pagination control', async () => {
    setupPaginatedMock(5);
    renderWithClient();

    await triggerSearch('test');

    const bottomPagination = getBottomPagination();
    const page3Button = within(bottomPagination!).getByRole('link', { name: 'Go to page 3' });
    await userEvent.click(page3Button);

    await waitFor(() => {
      expect(screen.getByText(/Movie Page 3/)).toBeInTheDocument();
    });
  });
});
