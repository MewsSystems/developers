import { AppThunk } from "../reducers"
import { MovieLoadErrorAction, MovieLoadStartedAction, MovieLoadSuccessAction, MOVIE_LOAD_STARTED, MOVIE_LOAD_ERROR, MOVIE_LOAD_SUCCESS } from '../constants/movie-detail-actions';
import { Movie } from "../services/types";
import { getMovie } from "../services/movie-service";

export const movieLoad = (id: number): AppThunk => async (dispatch) => {
  dispatch(movieLoadStarted());
  try {
    const movie = await getMovie(id);
    dispatch(movieLoadSuccess(movie));
  }
  catch (e) {
    dispatch(movieLoadError(e));
  }
}

export const movieLoadStarted = (): MovieLoadStartedAction => {
  return {
    type: MOVIE_LOAD_STARTED,
    payload: {
      isFetching: true,
      error: ''
    }
  }
}

export const movieLoadError = (error: string): MovieLoadErrorAction => {
  return {
    type: MOVIE_LOAD_ERROR,
    payload: {
      isFetching: false,
      error
    }
  }
}

export const movieLoadSuccess = (movie: Movie): MovieLoadSuccessAction => {
  return {
    type: MOVIE_LOAD_SUCCESS,
    payload: {
      isFetching: false,
      movie
    }
  }
}

export type MovieDetailActions = MovieLoadErrorAction | MovieLoadStartedAction | MovieLoadSuccessAction;
