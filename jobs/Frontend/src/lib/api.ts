import { MovieDetailsResponse, MovieSearchResponse } from "../types/custom";

const token = import.meta.env.VITE_TMDB_API_TOKEN;

export const queryMovies = async (query: string, page: string) => {
  const url = `https://api.themoviedb.org/3/search/movie?query=${query}&page=${page}`;
  const response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  const result: MovieSearchResponse = await response.json();

  return result;
};

export const queryMovieDetails = async (movieId: string) => {
  const url = `https://api.themoviedb.org/3/movie/${movieId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  const result: MovieDetailsResponse = await response.json();

  return result;
};
