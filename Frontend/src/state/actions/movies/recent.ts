import { Movie } from 'model/api/Movie'
import { createAction } from 'redux-actions'

export const ADD_RECENT_MOVIE = 'ADD_RECENT_MOVIE'

export const setFilter = createAction<Movie>(ADD_RECENT_MOVIE)
