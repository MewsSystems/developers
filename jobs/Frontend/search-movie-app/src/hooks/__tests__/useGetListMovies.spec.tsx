import { renderHook, act, waitFor } from '@testing-library/react';
import { vi } from 'vitest';
import { QueryClientWrapper } from '../../utils/testUtils/QueryClientWrapper';
import { fetchListMovies } from '../../api/fetch';
import { useGetListMovies } from '../useGetListMovies';

vi.mock('../../api/fetch/fetchListMovies');

describe('useGetListMovies hook', () => {
  const wrapper = ({ children }: { children: React.ReactNode }) => (
    <QueryClientWrapper>{children}</QueryClientWrapper>
  );

  beforeEach(() => {
    const fetchListMoviesMock = fetchListMovies as ReturnType<typeof vi.fn>;

    fetchListMoviesMock.mockReset();

    fetchListMoviesMock.mockResolvedValueOnce({
      page: 1,
      total_pages: 2,
      results: [{ id: 1, title: 'Batman' }],
    });

    fetchListMoviesMock.mockResolvedValueOnce({
      page: 2,
      total_pages: 2,
      results: [{ id: 2, title: 'Superman' }],
    });
  });

  it('fetches the first page', async () => {
    const { result } = renderHook(() => useGetListMovies({ query: 'man' }), { wrapper });
    await waitFor(() => expect(result.current.data?.pages[0].page).toBe(1));

    expect(result.current.data?.pages[0].results[0].title).toBe('Batman');
  });
  it('fetches the next page using the fetchNextPage function', async () => {
    const { result } = renderHook(() => useGetListMovies({ query: 'man' }), { wrapper });
    await waitFor(() => expect(result.current.data?.pages[0].page).toBe(1));
    act(() => result.current.fetchNextPage());
    await waitFor(() => expect(result.current.data?.pages[1].page).toBe(2));

    expect(result.current.data?.pages[1].results[0].title).toBe('Superman');
  });
});
