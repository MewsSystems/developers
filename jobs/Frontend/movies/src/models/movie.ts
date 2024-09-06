import { CountryCode } from "./country";
import { DateString } from "./dateString";
import { LanguageCode } from "./language";

export interface Movie {
  adult: boolean;
  backdrop_path: string | null;
  genre_ids: number[];
  id: number;
  original_language: LanguageCode;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  release_date: DateString;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface MovieResponse {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
}

interface Collection {
  id: number;
  name: string;
  poster_path: string;
  backdrop_path: string;
}

interface Genre {
  id: number;
  name: string;
}

export interface MovieDetails {
  adult: boolean;
  backdrop_path: Collection | null;
  belongs_to_collection: string | null;
  budget: number;
  genres: Genre[];
  homepage: string;
  id: number;
  imdb_id: string | null;
  origin_country: CountryCode[];
  original_language: LanguageCode;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  production_companies: Array<{
    id: number;
    logo_path: string | null;
    name: string;
    origin_country: CountryCode;
  }>;
  production_countries: Array<{
    iso_3166_1: CountryCode;
    name: string;
  }>;
  release_date: DateString;
  revenue: number;
  runtime: number;
  spoken_languages: Array<{
    iso_639_1: LanguageCode;
    name: string;
  }>;
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}
