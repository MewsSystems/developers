import { ADD_RECENT_MOVIE } from 'state/actions/movies/recent'
import { Movie } from 'model/api/Movie'
import { Action, handleActions } from 'redux-actions'

export interface RecentMoviesState {
  items: Movie[]
}

export const recentMoviesInitialState: RecentMoviesState = {
  items: [],
}

export const recentMoviesReducer = handleActions<RecentMoviesState, any>(
  {
    [ADD_RECENT_MOVIE]: (
      state: RecentMoviesState,
      action: Action<Movie>
    ): RecentMoviesState => {
      /* Initial items will show only last 10 movies */
      const newItems = [...state.items.slice(0, 9)]

      return {
        ...state,
        items: [action.payload, ...newItems],
      }
    },
  },
  recentMoviesInitialState
)
