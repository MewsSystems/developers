import { combineReducers } from "redux";
import browseMoviesReducer from "./BrowseMoviesReducer";

export default combineReducers({ browseMovies: browseMoviesReducer });
