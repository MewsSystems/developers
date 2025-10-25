export interface Movie {
  id: number
  title: string
  poster_path: string | null
  backdrop_path: string | null
  overview: string
  release_date: string
  vote_average: number
  vote_count: number
  popularity: number
  genre_ids?: number[]
}

export interface MovieDetail extends Movie {
  genres?: { id: number; name: string }[]
  runtime?: number
  status?: string
  tagline?: string
  budget?: number
  revenue?: number
  production_companies?: { id: number; name: string; logo_path: string | null }[]
  spoken_languages?: { iso_639_1: string; name: string }[]
}

export interface MoviesResponse {
  page: number
  results: Movie[]
  total_pages: number
  total_results: number
}

export interface Genre {
  id: number
  name: string
}
