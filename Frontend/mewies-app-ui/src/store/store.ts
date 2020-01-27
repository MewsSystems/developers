import { createStore, combineReducers, applyMiddleware } from 'redux'
import thunk from 'redux-thunk'
import { composeWithDevTools } from 'redux-devtools-extension'

import { dev } from '../utils/api/api.config'
import { app, AppState } from './app/app.reducer'
import { movies, MoviesState } from './movies/movies.reducer'

export interface ApplicationState {
    app: AppState
    movies: MoviesState
}

const rootReducer = combineReducers({ app, movies })

const middleware = dev
    ? composeWithDevTools(applyMiddleware(thunk))
    : applyMiddleware(thunk)

export const initStore = (initialStore: ApplicationState) => {
    return createStore(rootReducer, initialStore, middleware)
}
