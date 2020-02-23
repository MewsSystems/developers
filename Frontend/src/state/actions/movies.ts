import { createAction } from 'redux-actions'
import { ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'
import { State } from 'state/rootReducer'
import { Movie } from 'model/api/Movie'
import { MoviesApi } from 'api/Movies'
import { List } from 'model/api/List'
import { DATE_IN_PAST } from 'constants/date'

export interface MoviesState {
  trending: Pick<List, 'total_pages' | 'total_results'> & {
    results: {
      [key: number]: Movie[]
    }
  }
  updatedAt: Date
  loading: boolean
}

export interface HydrateTrendingMoviesPayload {
  data: List<Movie>
  rehydrate: boolean
}

export const moviesInitialState: MoviesState = {
  trending: {
    results: {},
    total_pages: 0,
    total_results: 0,
  },
  updatedAt: DATE_IN_PAST,
  loading: false,
}

export const REQUEST_DATA = 'REQUEST_DATA'
export const RECEIVE_DATA = 'RECEIVE_DATA'
export const HYDRATE_TRENDING_MOVIES = 'HYDRATE_TRENDING_MOVIES'
export const SET_UPDATED_AT = 'SET_UPDATED_AT'

export const requestData = createAction(REQUEST_DATA)
export const receiveData = createAction(RECEIVE_DATA)
export const hydrateTrendingMoviesAction = createAction<
  HydrateTrendingMoviesPayload
>(HYDRATE_TRENDING_MOVIES)
export const setUpdatedAt = createAction(SET_UPDATED_AT)

export const hydrateTrendingMovies = (
  rehydrate = false,
  page?: number
) => async (
  dispatch: ThunkDispatch<State, void, AnyAction>,
  getState: () => State
) => {
  const { results } = getState().movies.trending

  if (!results[page] || rehydrate) {
    dispatch(requestData())

    const { data } = await MoviesApi.getTrendingMovies(
      'week',
      rehydrate ? 1 : page
    )

    dispatch(hydrateTrendingMoviesAction({ data, rehydrate }))

    if (rehydrate) {
      dispatch(setUpdatedAt())
    }

    dispatch(receiveData())
  }
}
