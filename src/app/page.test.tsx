import { render, screen } from '@testing-library/react';
import HomePage from './page';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { http, HttpResponse } from 'msw';
import { server } from '@/test/server';

// Define explicit prop types for the mocked component
interface MockHomeSearchSectionProps {
  initialSearch: string;
  initialPage: number;
}

vi.mock('@/features/home/HomeSearchSection', () => ({
  HomeSearchSection: ({ initialSearch, initialPage }: MockHomeSearchSectionProps) => (
    <div>{`Mocked HomeSearchSection for "${initialSearch}" page ${initialPage}`}</div>
  ),
}));

beforeAll(() => {
  process.env.NEXT_PUBLIC_SITE_URL = 'http://localhost:3000';
});

afterAll(() => {
  delete process.env.NEXT_PUBLIC_SITE_URL;
});

async function renderWithQueryProvider(params: Record<string, string | undefined>) {
  const queryClient = new QueryClient();
  const jsx = await HomePage({ searchParams: Promise.resolve(params) });

  return render(<QueryClientProvider client={queryClient}>{jsx}</QueryClientProvider>);
}

describe('<HomePage />', () => {
  it('renders HomeSearchSection with default props if no searchParams', async () => {
    await renderWithQueryProvider({});

    expect(await screen.findByText(/Mocked HomeSearchSection for "" page 1/)).toBeInTheDocument();
  });

  it('renders HomeSearchSection with parsed params when search is present', async () => {
    server.use(
      http.get('http://localhost:3000/api/movies', ({ request }) => {
        const url = new URL(request.url);
        expect(url.searchParams.get('search')).toBe('matrix');
        expect(url.searchParams.get('page')).toBe('2');

        return HttpResponse.json({
          results: [],
          total_pages: 1,
          total_results: 0,
        });
      })
    );

    await renderWithQueryProvider({ search: 'matrix', page: '2' });

    expect(
      await screen.findByText(/Mocked HomeSearchSection for "matrix" page 2/)
    ).toBeInTheDocument();
  });

  it('falls back to page=1 if page is not a valid number', async () => {
    server.use(
      http.get('http://localhost:3000/api/movies', ({ request }) => {
        const url = new URL(request.url);
        expect(url.searchParams.get('search')).toBe('inception');
        expect(url.searchParams.get('page')).toBe('1');

        return HttpResponse.json({
          results: [],
          total_pages: 1,
          total_results: 0,
        });
      })
    );

    await renderWithQueryProvider({ search: 'inception', page: 'invalid' });

    expect(
      await screen.findByText(/Mocked HomeSearchSection for "inception" page 1/)
    ).toBeInTheDocument();
  });
});
