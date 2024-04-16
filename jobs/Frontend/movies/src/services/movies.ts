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
    )}&include_adult=false&language=en-US&&api_key=${import.meta.env.VITE_TMDB_API_KEY}`,
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

export interface MovieResponse {
  adult: boolean;
  backdrop_path: string;
  belongs_to_collection: null;
  budget: number;
  genres: Genre[];
  homepage: string;
  id: number;
  imdb_id: string;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  production_companies: ProductionCompany[];
  production_countries: ProductionCountry[];
  release_date: string;
  revenue: number;
  runtime: number;
  spoken_languages: SpokenLanguage[];
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface Genre {
  id: number;
  name: string;
}

export interface ProductionCompany {
  id: number;
  logo_path: null | string;
  name: string;
  origin_country: string;
}

export interface ProductionCountry {
  iso_3166_1: string;
  name: string;
}

export interface SpokenLanguage {
  english_name: string;
  iso_639_1: string;
  name: string;
}

export interface Movie {
  id: number;
  title: string;
  overview: string;
  genres: string[];
  productionCompanies: string[];
  languages: string[];
  releaseDate: string;
  rating: number;
  imgSrc: string;
  runtime: number;
}

export async function fetchMovie(id: string): Promise<Movie> {
  return fetch(
    `https://api.themoviedb.org/3/movie/${id}?language=en-US&api_key=${import.meta.env.VITE_TMDB_API_KEY}`,
  )
    .then((r) => r.json())
    .then(
      ({
        id,
        title,
        overview,
        vote_average,
        production_companies,
        spoken_languages,
        backdrop_path,
        runtime,
        genres,
        release_date,
      }: MovieResponse) => ({
        id,
        title,
        overview,
        rating: vote_average,
        runtime,
        productionCompanies: production_companies.map(({ name }) => name),
        languages: spoken_languages.map(({ name }) => name),
        imgSrc: `https://image.tmdb.org/t/p/original/${backdrop_path}`,
        genres: genres.map(({ name }) => name),
        releaseDate: release_date,
      }),
    );
}

export const findMovieQueryOptions = (id: string) =>
  queryOptions({
    queryKey: ["movies", id],
    queryFn: () => fetchMovie(id),
  });
