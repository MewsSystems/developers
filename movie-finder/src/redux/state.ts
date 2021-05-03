import { createStore } from "redux";
import { rootReducer } from "./reducer";
import { MoviesPage } from "../types";

export type AppReduxState = {
  searchMovieTitle: string;
  currentPage: number;
  loading: boolean;
  moviePages: MoviesPage[];
};

export const initialState: AppReduxState = {
  searchMovieTitle: "",
  currentPage: 1,
  loading: false,
  moviePages: [],
};

export const store = createStore(
  rootReducer,
  // @ts-ignore
  window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
);
