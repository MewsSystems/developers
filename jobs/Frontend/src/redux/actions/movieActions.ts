// Import required modules
import { getMovie, searchMovies } from "@services/movieService";
import { AnyAction, Dispatch } from "redux";
import { ThunkAction } from "redux-thunk";
import { RootState } from "../store";

// Define action types
export const FETCH_MOVIES = "FETCH_MOVIES";
export const FETCH_MOVIE_DETAIL = "FETCH_MOVIE_DETAIL";

// Define action creator function to fetch list of movies
export const fetchMovies =
  (
    query: string, // search query
    page: number = 1 // page number of search results (default: 1)
  ): ThunkAction<void, RootState, unknown, AnyAction> => // thunk action
  async (dispatch: Dispatch) => {
    try {
      // Call searchMovies function to get list of movies
      const movies = await searchMovies(query, page);

      // If movies are found, dispatch a success action
      if (movies.length > 0) {
        dispatch({
          type: FETCH_MOVIES,
          payload: { movies, query, page, status: "success" },
        });
      }

      // If no search query was provided, dispatch an idle action
      if (query.length === 0) {
        dispatch({
          type: FETCH_MOVIES,
          payload: { movies, query, page, status: "idle" },
        });
      }

      // If no movies are found and a search query was provided, dispatch an error action
      if (movies.length === 0 && query.length > 0) {
        dispatch({
          type: FETCH_MOVIES,
          payload: { movies: [], query, page, status: "error" },
        });
      }
    } catch (err) {
      // If an error occurs, dispatch an error action
      dispatch({
        type: FETCH_MOVIES,
        payload: { movies: [], query, page, status: "error" },
      });
    }
  };

// Define action creator function to fetch details of a movie
export const fetchMovieDetail =
  (
    id: string
  ): ThunkAction<void, RootState, unknown, AnyAction> => // thunk action
  async (dispatch: Dispatch) => {
    // Call getMovie function to get details of the movie
    const movie = await getMovie(id);

    // Dispatch an action with the movie details payload
    dispatch({ type: FETCH_MOVIE_DETAIL, payload: movie });
  };
