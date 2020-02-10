import { MovieDetailActions } from "../actions/movie-detail"
import { Movie } from "../services/types"

export type MovieDetailState = {
  movie?: Movie,
  error: string;
  isFetching: boolean,
}

const initialState: MovieDetailState = {
  movie: undefined,
  error: '',
  isFetching: false,
}

export const movieDetailReducer = (state: MovieDetailState = initialState, action: MovieDetailActions): MovieDetailState => {
  switch (action.type) {
    case "MOVIE_LOAD_ERROR": {
      return {
        ...state,
        ...action.payload,
      }
    }
    case "MOVIE_LOAD_STARTED": {
      return {
        ...state,
        ...action.payload
      }
    }
    case "MOVIE_LOAD_SUCCESS": {
      return {
        ...state,
        ...action.payload
      }
    }
    default:
      return state;
  }
}

