import { combineReducers } from 'redux';
import AllMovieReducer from './AllMovieReducer';
import MovieMultipleReducer from './MovieMultipleReducer';
import MovieListReducer from './MovieListReducer';

const RootReducer = combineReducers({
  AllMovie: AllMovieReducer,
  Movie: MovieMultipleReducer,
  MovieList: MovieListReducer,
});

export default RootReducer;
