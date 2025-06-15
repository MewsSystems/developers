import { vi, describe, it, expect } from 'vitest';
import { fetchListMovies } from '../fetchListMovies';
import { instance } from '../../instance';
import type { ListMoviesParams, ListMoviesResponse } from '../../types';
import { MOCKED_AXIOS_ERROR, MOCKED_LIST_MOVIES } from '../../constants';

vi.mock('../../instance');

const mockedInstance = instance as jest.Mocked<typeof instance>;

describe('Verify positive and negative scenarios for fetchListMovies', () => {
  const mockResponse: ListMoviesResponse = {
    page: 1,
    results: [MOCKED_LIST_MOVIES],
    total_pages: 1,
    total_results: 1,
  };

  it('should fetch movies with correct params and return data', async () => {
    mockedInstance.get.mockResolvedValueOnce({ data: mockResponse });

    const params: ListMoviesParams = { query: 'test', page: 1 };
    const result = await fetchListMovies(params);

    expect(instance.get).toHaveBeenCalledWith('/search/movie', {
      params: { query: 'test', page: 1 },
    });
    expect(result).toEqual(mockResponse);
  });

  it('should throw an axios error when the request fails', async () => {
    const {
      response: {
        data: { status_code, status_message },
      },
    } = MOCKED_AXIOS_ERROR;
    mockedInstance.get.mockRejectedValueOnce(MOCKED_AXIOS_ERROR);

    await expect(fetchListMovies({ query: 'fail' })).rejects.toEqual({
      status: status_code,
      message: status_message,
    });
    expect(instance.get).toHaveBeenCalledWith('/search/movie', {
      params: { query: 'fail', page: 1 },
    });
  });

  it('should throw a general error when the request fails', async () => {
    const error = new Error('Network error');
    mockedInstance.get.mockRejectedValueOnce(error);

    await expect(fetchListMovies({ query: 'fail' })).rejects.toEqual({
      message: 'Network error',
    });
    expect(instance.get).toHaveBeenCalledWith('/search/movie', {
      params: { query: 'fail', page: 1 },
    });
  });
});
