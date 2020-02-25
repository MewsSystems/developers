// This file holds our state type, as well as any other types related to this Redux store.

// This type is basically shorthand for `{ [key: string]: any }`. Feel free to replace `any` with
// the expected return type of your API response.
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export type ApiResponse = Record<string, any>

// Response object for GET /movies
export type MovieIndexItem = {
  id: number
  popularity: number
  video: boolean
  vote_count: number
  vote_average: string
  title: string
  release_date: string
  original_language: string
  original_title: string
  genre_ids: number[]
  backdrop_path: string
  adult: boolean
  overview: string
  poster_path: string
}

export interface Page extends ApiResponse {
  page: number
  total_results: number
  total_pages: number
  results: MovieIndexItem[]
}

// Use `enum`s for better autocompletion of action type names. These will
// be compiled away leaving only the final value in your compiled code.
//
// Define however naming conventions you'd like for your action types, but
// personally, I use the `@@context/ACTION_TYPE` convention, to follow the convention
// of Redux's `@@INIT` action.
export enum MovieIndexActionTypes {
  PAGE_CHANGED = '@@movies/PAGE_CHANGED',
  SEARCH_CHANGED = '@@movies/SEARCH_CHANGED',
  FETCH_REQUEST = '@@movies/FETCH_REQUEST',
  FETCH_SUCCESS = '@@movies/FETCH_SUCCESS',
  FETCH_ERROR = '@@movies/FETCH_ERROR'
}

// Declare state types with `readonly` modifier to get compile time immutability.
// https://github.com/piotrwitek/react-redux-typescript-guide#state-with-type-level-immutability
export interface MovieIndexState {
  readonly search?: string
  readonly page?: number
  readonly loading: boolean
  readonly data?: Page
  readonly errors?: string
}
