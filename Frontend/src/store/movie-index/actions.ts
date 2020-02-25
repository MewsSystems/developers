import { action } from 'typesafe-actions'
import { MovieIndexActionTypes, MovieIndexItem } from './types'

// Here we use the `action` helper function provided by `typesafe-actions`.
// This library provides really useful helpers for writing Redux actions in a type-safe manner.
// For more info: https://github.com/piotrwitek/typesafe-actions
export const searchChange = (search: string) => action(MovieIndexActionTypes.SEARCH_CHANGED, search)
export const pageChange = (index: number) => action(MovieIndexActionTypes.PAGE_CHANGED, index)
export const fetchSearchRequest = (search: string) => action(MovieIndexActionTypes.FETCH_REQUEST, search)
export const fetchSearchRequestSuccess = (data: MovieIndexItem[]) => action(MovieIndexActionTypes.FETCH_SUCCESS, data)
export const fetchSearchRequestError = (message: string) => action(MovieIndexActionTypes.FETCH_ERROR, message)
