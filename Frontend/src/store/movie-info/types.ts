import { ApiResponse } from '../movie-index/types'

export type MovieCollection = {
  id: number
  name: string
  poster_path: string
  backdrop_path: string
}

export type MovieGenre = {
  id: number
  name: string
}

export type MovieProductionCompany = {
  id: number
  logo_path: string
  name: string
  origin_country: string
}

export type MovieProductionCountry = {
  iso_3166_1: string
  name: string
}

export type MovieSpokenLanguage = {
  iso_639_1: string
  name: string
}

export interface MovieInfo extends ApiResponse {
  id: number
  adult: boolean
  backdrop_path: string
  belongs_to_collection: MovieCollection

  poster_path: string
  title: string
  genres: MovieGenre[]
  overview: string

  vote_count: number
  vote_average: number
  popularity: string

  release_date: string
  budget: number
  revenue: number
  runtime: number

  spoken_languages: MovieSpokenLanguage[]
  original_language: string
  original_title: string
  tagline: string

  production_companies: MovieProductionCompany[]
  production_countries: MovieProductionCountry[]
  homepage: string
  imdb_id: string

  status: string
  video: boolean
}

// Declare state types with `readonly` modifier to get compile time immutability.
// https://github.com/piotrwitek/react-redux-typescript-guide#state-with-type-level-immutability
export interface MovieInfoState {
  readonly loading: boolean
  readonly data?: MovieInfo
  readonly errors?: string
}

// Use `enum`s for better autocompletion of action type names. These will
// be compiled away leaving only the final value in your compiled code.
//
// Define however naming conventions you'd like for your action types, but
// personally, I use the `@@context/ACTION_TYPE` convention, to follow the convention
// of Redux's `@@INIT` action.
export enum MovieInfoActionTypes {
  FETCH_REQUEST = '@@movie/FETCH_REQUEST',
  FETCH_SUCCESS = '@@movie/FETCH_SUCCESS',
  FETCH_ERROR = '@@movie/FETCH_ERROR'
}
