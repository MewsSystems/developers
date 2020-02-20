import {
  RecentMoviesState,
  recentMoviesInitialState,
  recentMoviesReducer,
} from 'state/reducers/movies/recent'
import { combineReducers } from 'redux'
import { PersistState } from 'redux-persist'

export interface State {
  recentMovies: RecentMoviesState
  _persist?: PersistState
}

export const initialState: State = {
  recentMovies: recentMoviesInitialState,
}

export const rootReducer = combineReducers({
  recentMovies: recentMoviesReducer,
})
