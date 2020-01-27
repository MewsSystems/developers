import produce from 'immer'
import { MovieConstants } from './movies.constants'
import { FetchMovies } from './movies.actions'
import { MovieModel } from '../../utils/types/model'

type MovieActions = FetchMovies

export interface MoviesState {
    results: MovieModel[]
}

const initialState: MoviesState = {
    results: [],
}

export const movies = (state = initialState, action: MovieActions) =>
    // I used Immer to prevent mutability without being forced to copy state object every time.
    // Immer perfectly reduces amount code in reducers, it's safe and makes code much more readable.
    produce(state, (draft: MoviesState) => {
        switch (action.type) {
            case MovieConstants.FetchMovies:
                draft.results = action.movies
                break
        }
    })
