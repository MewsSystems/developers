import { findIndex } from "lodash";
import { Reducer } from "redux";
import { AppReduxState, initialState } from "./state";
import { ActionTypes, AppActions } from "./types";

export const rootReducer: Reducer<AppReduxState, AppActions> = (
  state = initialState,
  action
) => {
  switch (action.type) {
    case ActionTypes.SET_LOADING:
      return { ...state, loading: action.payload };
    case ActionTypes.SET_SEARCH_MOVIE_TITLE:
      return { ...state, searchMovieTitle: action.payload };
    case ActionTypes.SET_CURRENT_PAGE:
      return { ...state, currentPage: action.payload };
    case ActionTypes.SET_TOTAL_PAGES:
      return { ...state, totalPages: action.payload };
    case ActionTypes.SET_MOVIE_PAGE:
      const index = findIndex(state.moviePages, action.payload);
      const nextMoviePages =
        index !== -1 ? state.moviePages : [...state.moviePages, action.payload];
      return { ...state, moviePages: nextMoviePages };
    default:
      return state;
  }
};
