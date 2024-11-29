export type ApiResponse<T> = {
  results: T[]
  page: number
  total_results: number
  total_pages: number
}

export type ApiError = {
  message: string
  isError: true
}

export type Movie = {
  adult: boolean
  original_title: string
  poster_path: string
  id: number
  release_date: string
  title: string
  overview: string
  backdrop_path: string
  genre_ids: number[]
  original_language: string
  popularity: number
  video: boolean
  vote_average: number
  vote_count: number
}

export type MovieGenre = {
  id: string
  name: string
}

export type FullMovieResponse = {
  backdrop_path: string
  budget: 63000000
  poster_path: string
  genres: MovieGenre[]
  id: string
  original_title: string
  overview: string
  popularity: number
  release_date: string
  revenue: number
  runtime: number
  status: string
  tagline: string
  title: string
  video: false
  vote_average: number
  vote_count: number
}
