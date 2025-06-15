import { vi, describe, it, expect } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { useGetDetailsMovie } from '../useGetDetailsMovie';
import { fetchDetailsMovie } from '../../api/fetch';
import { QueryClientWrapper } from '../../utils/testUtils/QueryClientWrapper';
import { MOCKED_DETAILS_MOVIE } from '../../constants/mocks';

vi.mock('../../api/fetch');

const mockedFetchDetailsMovie = fetchDetailsMovie as jest.MockedFunction<typeof fetchDetailsMovie>;

describe('useGetDetailsMovie hook', () => {
  it('does not fetch when movieId is empty', () => {
    const { result } = renderHook(() => useGetDetailsMovie(''), {
      wrapper: QueryClientWrapper,
    });

    expect(result.current.isLoading).toBe(false);
    expect(result.current.isFetching).toBe(false);
    expect(mockedFetchDetailsMovie).not.toHaveBeenCalled();
  });
  it('fetches and returns movie details when movieId is provided', async () => {
    const movieId = '123';

    mockedFetchDetailsMovie.mockResolvedValueOnce(MOCKED_DETAILS_MOVIE);

    const { result } = renderHook(() => useGetDetailsMovie(movieId), {
      wrapper: QueryClientWrapper,
    });

    // Wait for the query to succeed
    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toEqual(MOCKED_DETAILS_MOVIE);
    expect(mockedFetchDetailsMovie).toHaveBeenCalledWith(movieId);
  });
});
