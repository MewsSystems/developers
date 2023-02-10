import { RootState } from "../app/store";
import { MoviesListState } from "../app/types";

export const selectMoviesState = (state: RootState): MoviesListState =>
  state.moviesList;
