import type {AxiosError} from 'axios';

import {client} from '../client';
import type {Movie} from '../types';

export const fetchMovieDetails = async (movieId: string): Promise<Movie> => {
  try {
    return (await client.get(`/movie/${movieId}`)).data;
  } catch (error) {
    const axiosError = error as AxiosError<{status_message: string}>;
    throw {
      status: axiosError.response?.status,
      message: axiosError.response?.data?.status_message || axiosError.message,
    };
  }
};
