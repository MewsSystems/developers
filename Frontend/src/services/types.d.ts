
export type MovieResult = {
  poster_path: string,
  adult: boolean
  overview: string,
  release_date: string,
  genre_ids: number[],
  id: number,
  original_title: string,
  original_language: string;
  title: string,
  backdrop_path: string,
  popularity: number,
  vote_count: number
  video: string,
  vote_average: number,
}

export type GenreResult = { id: number, name: string };

export type Movie = {
  adult: boolean,
  backdrop_path: string,
  belongs_to_collection?: string,
  budget: number,
  genres: GenreResult[],
  homepage: string,
  id: number,
  original_language: string,
  imdb_id: string,
  original_title: string,
  overview: string,
  popularity: number,
  poster_path: string,
  production_companies: {
    id: number,
    logo_path?: string,
    name: string
    origin_country: string,
  }[],
  production_countries: {
    iso_3166_1: string,
    name: string
  }[],
  release_date: string,
  revenue: number,
  runtime: number,
  spoken_languages: {
    iso_639_1: string,
    name: string
  }[],
  status: string,
  tagline: string,
  title: string,
  video: boolean,
  vote_average: number,
  vote_count: number
}


export type SearchParams = {
  query: string,
  page?: number,
  include_adult?: boolean,
  year?: number,
  primary_release_year?: number,
}
