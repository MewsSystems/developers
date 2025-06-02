import type {AxiosError} from 'axios';

import {client} from '../client';
import type {MovieSearchResponse} from '../types';

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
