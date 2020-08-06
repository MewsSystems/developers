export type Action = {
  type: string;
  payload: any;
};

type Company = {
  id: number;
  logo_path?: string;
  name: string;
  origin_country: string;
};

type Country = {
  iso_3166_1: string;
  name: string;
};

export type Movie = {
  adult: boolean;
  backdrop_path: string;
  genre_ids?: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path?: string;
  release_date?: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

export type MovieDetail = Movie & {
  belongs_to_collection: null;
  budget: number;
  homepage: string;
  genres: { id: number; name: string }[];
  imdb_id: string;
  production_companies: ProductionCompanies[];
  production_countries: Country[];
  revenue: number;
  runtime: number;
  spoken_languages: Country[];
  status: string;
  tagline?: string;
  loading: boolean;
};
