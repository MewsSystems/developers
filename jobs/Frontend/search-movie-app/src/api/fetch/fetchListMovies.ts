import { instance } from '../instance';
import type { ListMoviesParams, ListMoviesResponse } from '../types';
import { handleAxiosError } from '../utils/handleAxiosError';

const fetchListMovies = async ({
  query,
  page = 1,
}: ListMoviesParams): Promise<ListMoviesResponse> => {
  try {
    const response = await instance.get('/search/movie', {
      params: {
        query,
        page,
      },
    });
    return response.data;
  } catch (error) {
    return handleAxiosError(error);
  }
};

export { fetchListMovies };
