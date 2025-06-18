import { vi, describe, it, expect } from 'vitest';
import { instance } from '../../instance';
import { MOCKED_AXIOS_ERROR } from '../../constants';
import { MOCKED_LIST_MOVIES } from '../../../constants';
import { fetchPopularMovies } from '../fetchPopularMovies';
import type { ListMoviesResponse } from '../../types';

vi.mock('../../instance');

const mockedInstance = instance as jest.Mocked<typeof instance>;

describe('Verify positive and negative scenarios for fetchPopularMovies', () => {
  const mockResponse: ListMoviesResponse = MOCKED_LIST_MOVIES;

  it('should fetch popular movies and return data', async () => {
    mockedInstance.get.mockResolvedValueOnce({ data: mockResponse });

    const result = await fetchPopularMovies();

    expect(result).toEqual(mockResponse);
  });

  it('should throw an axios error when the request fails', async () => {
    const {
      response: {
        data: { status_code, status_message },
      },
    } = MOCKED_AXIOS_ERROR;
    mockedInstance.get.mockRejectedValueOnce(MOCKED_AXIOS_ERROR);

    await expect(fetchPopularMovies()).rejects.toEqual({
      status: status_code,
      message: status_message,
    });
  });

  it('should throw a general error when the request fails', async () => {
    const error = new Error('Network error');
    mockedInstance.get.mockRejectedValueOnce(error);

    await expect(fetchPopularMovies()).rejects.toEqual({
      message: 'Network error',
    });
  });
});
