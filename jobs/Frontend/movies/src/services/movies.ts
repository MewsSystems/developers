import { queryOptions } from "@tanstack/react-query";

export async function fetchMovies({
  page = 1,
  query = "",
}: {
  page?: number;
  query?: string;
}) {
  return fetch(
    `https://api.themoviedb.org/3/search/movie?page=${page}&query=${encodeURIComponent(
      query,
    )}&include_adult=false&language=en-US&page=1&api_key=${
      import.meta.env.VITE_TMDB_API_KEY
    }`,
  ).then((response) => response.json());
}

export const searchMoviesQueryOptions = (opts: {
  page?: number;
  query?: string;
}) =>
  queryOptions({
    queryKey: ["movies", opts],
    queryFn: () => fetchMovies(opts),
  });

export async function fetchMovie(id: string) {
  return fetch(
    `https://api.themoviedb.org/3/movie/${id}?language=en-US&api_key=${import.meta.env.VITE_TMDB_API_KEY}`,
  ).then((r) => r.json());
}

export const findMovieQueryOptions = (id: string) =>
  queryOptions({
    queryKey: ["movies", id],
    queryFn: () => fetchMovie(id),
  });
