import type { Genre } from "./movie-details-response"

export interface SimpleMovieDetails {
  image: string
  title: string
  tagline: string
  language: string
  length: number
  rate: number
  budget: number
  release_date: string
  genres: Genre[]
  overview: string
}
