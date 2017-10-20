import {combineReducers} from 'redux';
import config from './config';
import rates from './rates';
import filter from './filter';
import isWatching from './isWatching';


export default combineReducers({
  config,
  rates,
  filter,
  isWatching,
});
