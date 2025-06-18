import type { MovieDetails } from '../../types/movieTypes';
import { instance } from '../instance';
import { handleAxiosError } from '../utils/handleAxiosError';

export const fetchDetailsMovie = async (movieId: string): Promise<MovieDetails> => {
  try {
    const response = await instance.get(`/movie/${movieId}`);
    return response.data;
  } catch (error) {
    return handleAxiosError(error);
  }
};
