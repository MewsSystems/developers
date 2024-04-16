import { queryOptions } from "@tanstack/react-query";

export interface SearchResponse {
  page: number;
  results: SearchMovieResult[];
  total_pages: number;
  total_results: number;
}

export interface SearchMovieResult {
  adult: boolean;
  backdrop_path: null | string;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: Date;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface SearchResults {
  page: number;
  movies: MovieResult[];
  totalPages: number;
  totalResults: number;
}

export interface MovieResult {
  id: number;
  overview: string;
  title: string;
  rating: number;
  imgSrc: string;
}

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
    )}&include_adult=false&language=en-US&&api_key=${
      import.meta.env.VITE_TMDB_API_KEY
    }`,
  )
    .then((response) => response.json())
    .then(
      ({
        page,
        results,
        total_pages,
        total_results,
      }: SearchResponse): SearchResults => ({
        page,
        movies: results.map(
          ({ id, overview, title, vote_average, poster_path }) => ({
            id,
            overview,
            title,
            rating: vote_average,

            imgSrc: `https://image.tmdb.org/t/p/w500${poster_path}`,
          }),
        ),
        totalPages: total_pages,
        totalResults: total_results,
      }),
    );
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
