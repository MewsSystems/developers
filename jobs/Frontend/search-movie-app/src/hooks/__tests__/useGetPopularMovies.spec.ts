import { vi, describe, it, expect } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { fetchPopularMovies } from '../../api/fetch';
import { QueryClientWrapper } from '../../utils/testUtils/QueryClientWrapper';
import { MOCKED_LIST_MOVIES } from '../../constants';
import { useGetPopularMovies } from '../useGetPopularMovies';

vi.mock('../../api/fetch');

const mockedFetchPopularMovies = fetchPopularMovies as jest.MockedFunction<
  typeof fetchPopularMovies
>;

describe('useGetPopularMovies hook', () => {
  it('fetches and returns popular movies', async () => {
    mockedFetchPopularMovies.mockResolvedValueOnce(MOCKED_LIST_MOVIES);

    const { result } = renderHook(() => useGetPopularMovies(), {
      wrapper: QueryClientWrapper,
    });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toEqual(MOCKED_LIST_MOVIES);
  });
});
