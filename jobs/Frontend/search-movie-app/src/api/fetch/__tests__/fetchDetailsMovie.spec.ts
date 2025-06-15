import { vi, describe, it, expect } from 'vitest';
import { fetchDetailsMovie } from '../fetchDetailsMovie';
import { instance } from '../../instance';
import type { MovieDetails } from '../../types';
import { MOCKED_AXIOS_ERROR } from '../../constants';
import { MOCKED_DETAILS_MOVIE } from '../../../constants/mocks';

vi.mock('../../instance');

const mockedInstance = instance as jest.Mocked<typeof instance>;

const MOVIE_ID = 'Logan';

describe('Verify positive and negative scenarios for fetchDetailsMovie', () => {
  const mockResponse: MovieDetails = MOCKED_DETAILS_MOVIE;

  it('should fetch movies with correct params and return data', async () => {
    mockedInstance.get.mockResolvedValueOnce({ data: mockResponse });

    const result = await fetchDetailsMovie(MOVIE_ID);

    expect(instance.get).toHaveBeenCalledWith(`/movie/${MOVIE_ID}`);
    expect(result).toEqual(mockResponse);
  });

  it('should throw an axios error when the request fails', async () => {
    const {
      response: {
        data: { status_code, status_message },
      },
    } = MOCKED_AXIOS_ERROR;
    mockedInstance.get.mockRejectedValueOnce(MOCKED_AXIOS_ERROR);

    await expect(fetchDetailsMovie('fail')).rejects.toEqual({
      status: status_code,
      message: status_message,
    });
    expect(instance.get).toHaveBeenCalledWith(`/movie/fail`);
  });

  it('should throw a general error when the request fails', async () => {
    const error = new Error('Network error');
    mockedInstance.get.mockRejectedValueOnce(error);

    await expect(fetchDetailsMovie('fail')).rejects.toEqual({
      message: 'Network error',
    });
    expect(instance.get).toHaveBeenCalledWith(`/movie/fail`);
  });
});
