import { combineReducers } from "redux";
import movieDetailsReducer from "./movieDetails/movieDetailsReducer";
import movieSearchReducer from "./moviesSearch/movieSearchReducer";

export const rootReducer = combineReducers({
  moviesFound: movieSearchReducer,
  movieDetails: movieDetailsReducer,
});
