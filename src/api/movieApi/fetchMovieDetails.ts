import type {Movie} from './types';
import {movieApiService} from './movieApiService';
import type {AxiosError} from 'axios';

export const fetchMovieDetails = async (movieId: string): Promise<Movie> => {
  try {
    return (await movieApiService.get(`/movie/${movieId}`)).data;
  } catch (error) {
    const axiosError = error as AxiosError<{status_message: string}>;
    throw {
      status: axiosError.response?.status,
      message: axiosError.response?.data?.status_message || axiosError.message,
    };
  }
};
