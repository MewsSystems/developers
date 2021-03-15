import { AsyncThunkPayloadCreator } from '@reduxjs/toolkit';
import axios, { AxiosRequestConfig } from 'axios';

const API_KEY = process.env.REACT_APP_TMDB_API_KEY || '';
const API_URL_BASE = 'https://api.themoviedb.org/3/';

// create an axios instance and configure it to interact with the api
const tmdbAPI = axios.create({ baseURL: API_URL_BASE });
tmdbAPI.defaults.params = { api_key: API_KEY };

const endpoint = <R, P = void>(
  url: string | ((params: P) => string)
): AsyncThunkPayloadCreator<R, P> => async (params, { signal }) => {
  let config: AxiosRequestConfig = { params };

  if (signal) {
    const source = axios.CancelToken.source();
    config = { ...config, cancelToken: source.token };
    signal.addEventListener('abort', () => {
      source.cancel();
    });
  }

  const response = await tmdbAPI.get(
    typeof url === 'string' ? url : url(params),
    config
  );

  return response.data;
};

export interface ConfigurationData {
  images: {
    base_url: string;
    secure_base_url: string;
    backdrop_sizes: string[];
    poster_sizes: string[];
  };
}

export interface MovieDetail {
  id: number;
  title: string;
  original_title: string;
  original_language: string;
  release_date: string;
  overview: string;
  poster_path: string;
  vote_average: number;
  vote_count: number;
  genres: Array<{ id: number; name: string }>;
  runtime: number;
}

export interface MovieParams {
  movieId: string;
}

export interface SearchResults<T> {
  results: T[];
  page: number;
  total_results: number;
  total_pages: number;
}

export type SearchMovieItem = Omit<MovieDetail, 'genres'>;
export type SearchMovieResults = SearchResults<SearchMovieItem>;

export interface SearchMovieParams {
  query: string;
  page?: number;
}

// ----------
// Endpoints

export const getMovieSearchResults = endpoint<
  SearchMovieResults,
  SearchMovieParams
>('/search/movie');

export const getMovieDetail = endpoint<MovieDetail, MovieParams>(
  ({ movieId }) => `/movie/${movieId}`
);

export const getConfiguration = endpoint<ConfigurationData>('/configuration');
