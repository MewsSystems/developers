import { MovieDTO } from '@/modules/movies/infrastructure/http/dto/MovieDTO';

export interface MovieDetailDTO extends Omit<MovieDTO, 'genre_ids'> {
  belongs_to_collection?: BelongsToCollectionDTO;
  budget: number;
  genres: Array<GenreDTO>;
  homepage: string;
  id: number;
  imdb_id: string;
  production_companies: Array<ProductionCompanyDTO>;
  production_countries: Array<ProductionCountryDTO>;
  revenue: number;
  runtime: number;
  origin_country: Array<string>;
  spoken_languages: Array<SpokenLanguageDTO>;
  status: string;
  tagline: string;
  credits: {
    cast: Array<CastDTO>;
    crew: Array<CrewDTO>;
  };
}

interface BasePersonDTO {
  adult: boolean;
  gender: number;
  id: number;
  known_for_department: string;
  name: string;
  original_name: string;
  popularity: number;
  profile_path: string;
  credit_id: string;
}

export interface CastDTO extends BasePersonDTO {
  cast_id: number;
  character: string;
  order: number;
}

interface CrewDTO extends BasePersonDTO {
  department: string;
  job: string;
}

interface BelongsToCollectionDTO {
  id: number;
  name: string;
  poster_path: string;
  backdrop_path: string;
}

interface SpokenLanguageDTO {
  english_name: string;
  iso_639_1: string;
  name: string;
}

interface ProductionCompanyDTO {
  id: number;
  logo_path: string;
  name: string;
  origin_country: string;
}

interface ProductionCountryDTO {
  iso_3166_1: string;
  name: string;
}

interface GenreDTO {
  id: number;
  name: string;
}
