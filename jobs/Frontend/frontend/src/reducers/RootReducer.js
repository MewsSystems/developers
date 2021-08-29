import { combineReducers } from 'redux';
import MovieListReducer from './MovieListReducer';

const RootReducer = combineReducers({
  MovieList: MovieListReducer,
});

export default RootReducer;
