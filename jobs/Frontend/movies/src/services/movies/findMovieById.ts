import { queryOptions } from "@tanstack/react-query";

export interface MovieResponse {
  adult: boolean;
  backdrop_path: string;
  belongs_to_collection?: BelongsToCollection;
  budget: number;
  genres: Genre[];
  homepage: string;
  id: number;
  imdb_id: string;
  origin_country?: string[];
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

export interface BelongsToCollection {
  id: number;
  name: string;
  poster_path: string;
  backdrop_path: string;
}

export interface Genre {
  id: number;
  name: string;
}

export interface ProductionCompany {
  id: number;
  logo_path?: string;
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

export async function fetchMovieById(id: string): Promise<Movie> {
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

export const findMovieByIdQueryOptions = (id: string) =>
  queryOptions({
    queryKey: ["movies", id],
    queryFn: () => fetchMovieById(id),
  });
