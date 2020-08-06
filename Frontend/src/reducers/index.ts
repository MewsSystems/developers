import { combineReducers } from 'redux';
import search, { SearchReducer } from './search';
import movie from './movie';
import { MovieDetail } from '../types';

export default combineReducers({
  search,
  movie,
});

export type RootReducer = {
  search: SearchReducer;
  movie: MovieDetail;
};
