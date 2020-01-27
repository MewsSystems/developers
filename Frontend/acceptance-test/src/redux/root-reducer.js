import { combineReducers } from "redux";

//Reducers
import directoryReducer from "./directory/directory.reducer";
import movieReducer from "./movie/movie.reducer";
import searchMovieReducer from "./search-movie/search-movie.reducer";

const rootReducer = combineReducers({
  directory: directoryReducer,
  movie: movieReducer,
  searchMovie: searchMovieReducer
});

export default rootReducer;
