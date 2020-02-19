import { combineReducers } from 'redux';
import { connectRouter } from 'connected-react-router';
import moviesReducer from './movies/MoviesReducer';
import movieDetailsReducer from './movieDetails/MovieDetailsReducer';

const createRootReducer = (history) => combineReducers({
  router: connectRouter(history),
  moviesReducer: moviesReducer,
  movieDetailsReducer: movieDetailsReducer,
});

export default createRootReducer;
