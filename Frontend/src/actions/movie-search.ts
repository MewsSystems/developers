import { AppThunk, AppState } from "../reducers";
import { search } from "../services/movie-service";
import { MovieSearchStartedAction, MOVIE_SEARCH_STARTED, MovieSearchErrorAction, MOVIE_SEARCH_ERROR, MovieSearchSuccessAction, MOVIE_SEARCH_SUCCESS, PaginationChangePageAction, PAGINATION_CHANGE_PAGE } from "../constants/movie-search-actions";

export const movieSearch = (query: string, page?: number): AppThunk => async (dispatch, getState: () => AppState) => {
  dispatch(movieSearchStarted(query));
  try {
    const results = await search({ query, page });
    dispatch(movieSearchSuccess(results));
  }
  catch (e) {
    dispatch(movieSearchError(e));
  }
}

export const movieSearchStarted = (query: string): MovieSearchStartedAction => {
  return {
    type: MOVIE_SEARCH_STARTED,
    payload: { isFetching: true, query, error: '' }
  }
}

export const movieSearchError = (error: string): MovieSearchErrorAction => {
  return {
    type: MOVIE_SEARCH_ERROR,
    payload: { error, isFetching: false }
  }
}

export const movieSearchSuccess = (payload: PromiseReturnType<ReturnType<typeof search>>): MovieSearchSuccessAction => {
  return {
    type: MOVIE_SEARCH_SUCCESS,
    payload: {
      isFetching: false,
      ...payload
    }
  }
}

export function paginationChangePage(page: number): PaginationChangePageAction {
  return {
    type: PAGINATION_CHANGE_PAGE,
    payload: { page }
  }
}

export type MovieSearchActions = MovieSearchStartedAction | MovieSearchErrorAction | MovieSearchSuccessAction | PaginationChangePageAction;
