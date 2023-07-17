import { GetMoviesResponse, MovieDetails } from "./containers/Movies/types";

const API_KEY = import.meta.env.VITE_API_KEY;

export const baseFetch = <T = void>(url: string, config: RequestInit = {}) => {
  config.headers = Object.assign(config.headers || {}, {
    accept: "application/json",
    "content-type": "application/json",
  });

  return fetch(`/api${url}`, config).then(async (response) => {
    if (!response.ok) {
      const err = await response.text();
      throw new Error(err);
    }

    return (await response.json()) as T;
  });
};

export const getMovies = (query = "", page = 1) =>
  baseFetch<GetMoviesResponse>(
    `/search/movie?${new URLSearchParams({
      ...(query ? { query } : {}),
      adult: String(false),
      page: String(page),
      api_key: API_KEY,
    }).toString()}`
  );

export const getMovieDetail = (movieId: string) =>
  baseFetch<MovieDetails>(
    `/movie/${movieId}?${new URLSearchParams({
      api_key: API_KEY,
    }).toString()}`
  );
