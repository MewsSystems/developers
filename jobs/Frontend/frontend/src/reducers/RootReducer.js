import { combineReducers } from 'redux';
import MovieListReducer from './MovieListReducer';
import MovieMultipleReducer from './MovieMultipleReducer';

const RootReducer = combineReducers({
  MovieList: MovieListReducer,
  Movie: MovieMultipleReducer,
});

export default RootReducer;
