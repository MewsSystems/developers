import { combineReducers } from 'redux'
import { PersistState } from 'redux-persist'
import {
  ConfigurationState,
  configurationInitialState,
} from './actions/configuration'
import { configurationReducer } from './reducers/configuration'
import { MoviesState, moviesInitialState } from './actions/movies'
import { moviesReducer } from './reducers/movies'

export interface State {
  configuration: ConfigurationState
  movies: MoviesState
  _persist?: PersistState
}

export const initialState: State = {
  configuration: configurationInitialState,
  movies: moviesInitialState,
}

export const rootReducer = combineReducers({
  configuration: configurationReducer,
  movies: moviesReducer,
})
