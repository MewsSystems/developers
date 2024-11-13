import type { CountryCode } from "./country";
import type { DateString } from "./dateString";
import type { LanguageCode } from "./language";
import type { Collection, Genre } from "./movie";

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
