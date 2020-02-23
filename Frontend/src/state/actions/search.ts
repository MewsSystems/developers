import { createAction } from 'redux-actions'

export interface SearchState {
  value: string
}

export const searchInitialState: SearchState = {
  value: '',
}

export const SET_SEARCH_VALUE = 'SET_SEARCH_VALUE'

export const setSearchValue = createAction<string>(SET_SEARCH_VALUE)
