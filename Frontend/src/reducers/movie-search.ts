import { MovieSearchActions } from "../actions/movie-search";
import { MovieResult, GenreResult } from "../services/types";

export type MovieSearchState = {
  query: string,
  results: MovieResult[],
  page: number,
  total_pages: number,
  total_results: number,
  genres: GenreResult[],
  error?: string,
  isFetching?: boolean,
  selectedMovie?: number,
}

const initialState: MovieSearchState = {
  query: '',
  results: [],
  page: 0,
  total_pages: 0,
  total_results: 0,
  genres: [],
  isFetching: false,
}

export const movieSearchReducer = (state: MovieSearchState = initialState, action: MovieSearchActions): MovieSearchState => {
  switch (action.type) {
    case "MOVIE_SEARCH_STARTED": {
      return {
        ...state,
        ...action.payload,
      }
    }
    case "MOVIE_SEARCH_ERROR": {
      return {
        ...state,
        ...action.payload
      }
    }
    case "MOVIE_SEARCH_SUCCESS": {
      return {
        ...state,
        ...action.payload
      }
    }
    case "PAGINATION_CHANGE_PAGE": {
      return {
        ...state,
        ...action.payload
      }
    }
    default:
      return state;
  }
}

