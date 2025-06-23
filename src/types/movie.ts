export interface Movie {
  id: number
  title: string
  overview: string
  poster_path: string | null
  backdrop_path: string | null
  release_date: string
  vote_average: number
  vote_count: number
  genre_ids?: number[]
  adult?: boolean
  original_language?: string
  original_title?: string
  popularity?: number
  video?: boolean
}

export interface MovieDetails extends Movie {
  genres?: Genre[]
  runtime?: number
  production_companies?: ProductionCompany[]
  belongs_to_collection?: Collection | null
  budget?: number
  homepage?: string
  imdb_id?: string
  origin_country?: string[]
  production_countries?: Country[]
  revenue?: number
  spoken_languages?: SpokenLanguage[]
  status?: string
  tagline?: string
}

export interface Genre {
  id: number
  name: string
}

export interface ProductionCompany {
  id: number
  name: string
  logo_path?: string | null
  origin_country?: string
}

export interface Collection {
  id: number
  name: string
  poster_path: string | null
  backdrop_path: string | null
}

export interface Country {
  iso_3166_1: string
  name: string
}

export interface SpokenLanguage {
  english_name: string
  iso_639_1: string
  name: string
}

export interface PaginatedResponse<T> {
  page: number
  results: T[]
  total_pages: number
  total_results: number
}

export type MovieSearchResponse = PaginatedResponse<Movie>
export type MovieListResponse = PaginatedResponse<Movie>
