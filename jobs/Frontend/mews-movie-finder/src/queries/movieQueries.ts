import { Movie, MovieDetail } from "../types/movies";
import { PaginatedResponse } from "../types/requests";

export const getMovies = (
  search: string,
  page: number
): Promise<PaginatedResponse<Movie>> => {
  return fetch(
    `https://api.themoviedb.org/3/search/movie?query=${search}&page=${page}&api_key=${
      import.meta.env.VITE_TMDB_API_KEY
    }`
  )
    .then((result) => {
      if (result.ok) {
        return result.json() as Promise<PaginatedResponse<Movie>>;
      }
      throw new Error(result.statusText);
    })
    .catch((error) => {
      throw error;
    });
};

export const getMovieDetails = (
  movieId: number
): Promise<MovieDetail> => {
  return fetch(
    `https://api.themoviedb.org/3/movie/${movieId}?api_key=${
      import.meta.env.VITE_TMDB_API_KEY
    }`
  )
    .then((result) => {
      if (result.ok) {
        return result.json() as Promise<MovieDetail>;
      }
      throw new Error(result.statusText);
    })
    .catch((error) => {
      throw error;
    });
};
