/**
 * Data types for working with TMDB API
 */

export interface Movie {
  id: number;
  title: string;
  poster_path: string | null;
  backdrop_path: string | null;
  overview: string;
  release_date: string;
  vote_average: number;
  vote_count: number;
  popularity: number;
  original_language: string;
  genre_ids: Array<number>;
  adult: boolean;
  original_title: string;
  video: boolean;
}

export interface MovieDetails extends Movie {
  budget: number;
  genres: Array<Genre>;
  homepage: string | null;
  imdb_id: string | null;
  production_companies: Array<ProductionCompany>;
  production_countries: Array<ProductionCountry>;
  revenue: number;
  runtime: number | null;
  spoken_languages: Array<SpokenLanguage>;
  status: string;
  tagline: string | null;
  credits?: Credits;
  videos?: VideoResponse;
  images?: ImagesResponse;
}

export interface Genre {
  id: number;
  name: string;
}

export interface ProductionCompany {
  id: number;
  logo_path: string | null;
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

export interface Credits {
  cast: Array<Cast>;
  crew: Array<Crew>;
}

export interface Cast {
  adult: boolean;
  gender: number | null;
  id: number;
  known_for_department: string;
  name: string;
  original_name: string;
  popularity: number;
  profile_path: string | null;
  cast_id: number;
  character: string;
  credit_id: string;
  order: number;
}

export interface Crew {
  adult: boolean;
  gender: number | null;
  id: number;
  known_for_department: string;
  name: string;
  original_name: string;
  popularity: number;
  profile_path: string | null;
  credit_id: string;
  department: string;
  job: string;
}

export interface VideoResponse {
  results: Array<Video>;
}

export interface Video {
  iso_639_1: string;
  iso_3166_1: string;
  name: string;
  key: string;
  site: string;
  size: number;
  type: string;
  official: boolean;
  published_at: string;
  id: string;
}

export interface ImagesResponse {
  backdrops: Array<Image>;
  logos: Array<Image>;
  posters: Array<Image>;
}

export interface Image {
  aspect_ratio: number;
  height: number;
  iso_639_1: string | null;
  file_path: string;
  vote_average: number;
  vote_count: number;
  width: number;
}

export interface MovieResponse {
  page: number;
  results: Array<Movie>;
  total_pages: number;
  total_results: number;
}