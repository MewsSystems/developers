import { combineReducers } from 'redux'
import { PersistState } from 'redux-persist'
import {
  ConfigurationState,
  configurationInitialState,
} from './actions/configuration'
import { configurationReducer } from './reducers/configuration'
import { MoviesState, moviesInitialState } from './actions/movies'
import { moviesReducer } from './reducers/movies'
import { searchInitialState, SearchState } from './actions/search'
import { searchReducer } from './reducers/search'

export interface State {
  configuration: ConfigurationState
  movies: MoviesState
  search: SearchState
  _persist?: PersistState
}

export const initialState: State = {
  configuration: configurationInitialState,
  movies: moviesInitialState,
  search: searchInitialState,
}

export const rootReducer = combineReducers({
  configuration: configurationReducer,
  movies: moviesReducer,
  search: searchReducer,
})
