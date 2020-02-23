import { Action, handleActions } from 'redux-actions'
import {
  HYDRATE_TRENDING_MOVIES,
  RECEIVE_DATA,
  REQUEST_DATA,
  SET_UPDATED_AT,
  MoviesState,
  HydrateTrendingMoviesPayload,
  moviesInitialState,
} from 'state/actions/movies'

export const moviesReducer = handleActions<MoviesState, any>(
  {
    [REQUEST_DATA]: (state: MoviesState): MoviesState => ({
      ...state,
      loading: true,
    }),
    [RECEIVE_DATA]: (state: MoviesState): MoviesState => ({
      ...state,
      loading: false,
    }),
    [SET_UPDATED_AT]: (state: MoviesState): MoviesState => ({
      ...state,
      updatedAt: new Date(),
    }),
    [HYDRATE_TRENDING_MOVIES]: (
      state: MoviesState,
      action: Action<HydrateTrendingMoviesPayload>
    ): MoviesState => {
      const {
        data: { page, total_pages, total_results, results },
        rehydrate,
      } = action.payload

      if (rehydrate) {
        return {
          ...state,
          trending: {
            results: {
              [page]: results,
            },
            total_pages,
            total_results,
          },
        }
      } else {
        return {
          ...state,
          trending: {
            ...state.trending,
            results: {
              ...state.trending.results,
              [page]: results,
            },
          },
        }
      }
    },
  },
  moviesInitialState
)
