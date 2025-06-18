import { instance } from '../instance';
import type { ListMoviesResponse } from '../types';
import { handleAxiosError } from '../utils/handleAxiosError';

export const fetchPopularMovies = async (): Promise<ListMoviesResponse> => {
  try {
    const response = await instance.get(`/movie/popular`);
    return response.data;
  } catch (error) {
    return handleAxiosError(error);
  }
};
