import { action } from 'typesafe-actions'
import { MovieInfoActionTypes, MovieInfo } from './types'

// Here we use the `action` helper function provided by `typesafe-actions`.
// This library provides really useful helpers for writing Redux actions in a type-safe manner.
// For more info: https://github.com/piotrwitek/typesafe-actions
export const fetchInfoRequest = (id: number) => action(MovieInfoActionTypes.FETCH_REQUEST, id)
export const fetchInfoRequestSuccess = (data: MovieInfo) => action(MovieInfoActionTypes.FETCH_SUCCESS, data)
export const fetchInfoRequestError = (message: string) => action(MovieInfoActionTypes.FETCH_ERROR, message)
