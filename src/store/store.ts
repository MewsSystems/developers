import { createStore } from "redux";
import {
  SET_GENRES,
  SET_SELECTED_MOVIE,
  SET_SELECTED_MOVIE_GENRES
} from "../models/actions";
import { InitialState } from "../models/initialState";

const initialState: InitialState = {
  genres: [],
  selectedMovie: null,
  selectedMovieGenres: []
};

const counterReducer = (state = initialState, action) => {
  switch (action.type) {
    case SET_GENRES:
      return { ...state, genres: action.value };
    case SET_SELECTED_MOVIE:
      return { ...state, selectedMovie: action.value };
    case SET_SELECTED_MOVIE_GENRES:
      return { ...state, selectedMovieGenres: action.value };
    default:
      break;
  }
  return state;
};

const store = createStore(counterReducer);

export default store;
