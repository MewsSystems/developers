import { combineReducers } from 'redux';
import { connectRouter } from 'connected-react-router';
import moviesReducer from './movies/MoviesReducer';

const createRootReducer = (history) => combineReducers({
  router: connectRouter(history),
  moviesReducer: moviesReducer
});

export default createRootReducer;
