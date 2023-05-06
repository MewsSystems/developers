import { combineReducers } from "redux";
import movieSearchReducer from "./moviesSearch/movieSearchReducer";

export const rootReducer = combineReducers({
  moviesFound: movieSearchReducer,
});
