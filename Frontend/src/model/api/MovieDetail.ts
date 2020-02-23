import { MovieSpokenLanguage } from './MovieSpokenLanguage'
import { MovieProductionCountry } from './MovieProductionCountry'
import { MovieProductionCompany } from './MovieProductionCompany'
import { MovieGenre } from './MovieGenre'

export interface MovieDetail {
  adult: boolean
  backdrop_path: string | null
  belongs_to_collection: any
  budget: number
  genres: MovieGenre[]
  homepage: string | null
  id: number
  imdb_id: number | null
  original_language: string
  original_title: string
  overview: string | null
  popularity: number
  poster_path: string | null
  production_companies: MovieProductionCompany[]
  production_countries: MovieProductionCountry[]
  release_date: string
  revenue: number
  runtime: number | null
  spoken_languages: MovieSpokenLanguage[]
  status: string
  tagline: string | null
  title: string
  video: boolean
  vote_average: number
  vote_count: number
}
