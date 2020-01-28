import { MovieConstants } from './movies.constants'
import { Dispatch } from 'redux'
import { AsyncActionType, endLoading, startLoading } from '../app/app.actions'
import { getMoviesByQuery } from '../../utils/api/movie.requests'
import { MovieModel } from '../../utils/types/model'
import { mapMovieDtoToModel } from '../../utils/mappers/movie.mappers'

export interface FetchMovies {
    type: MovieConstants.FetchMovies
    movies: MovieModel[]
}

export const fetchMovies = (movies: MovieModel[]) => {
    return {
        type: MovieConstants.FetchMovies,
        movies,
    }
}

export const submitSearchQuery = (query: string) => {
    return async (dispatch: Dispatch<AsyncActionType | FetchMovies>) => {
        dispatch(startLoading())
        try {
            const { results } = await getMoviesByQuery(query)
            const mappedResults = results.map(mapMovieDtoToModel)
            dispatch(fetchMovies(mappedResults))
            dispatch(endLoading())
        } catch (e) {
            dispatch(endLoading())
        }
    }
}
