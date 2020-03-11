import { combineReducers } from 'redux';
import detail from './detail';
import movies from './movies';

export default combineReducers({
  movies,
  detail
});
