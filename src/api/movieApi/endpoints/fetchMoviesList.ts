import type {MovieSearchResponse} from '../types';
import {client} from '../client';
import type {AxiosError} from 'axios';

export const fetchMoviesList = async (
  query: string,
  page: number = 1,
): Promise<MovieSearchResponse> => {
  try {
    return (
      await client.get('/search/movie', {
        params: {
          query,
          page,
        },
      })
    ).data;
  } catch (error) {
    const axiosError = error as AxiosError<{status_message: string}>;
    throw {
      status: axiosError.response?.status,
      message: axiosError.response?.data?.status_message || axiosError.message,
    };
  }
};
