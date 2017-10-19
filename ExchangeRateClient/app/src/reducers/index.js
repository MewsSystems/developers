import {combineReducers} from 'redux';
import config from './config';
import rates from './rates';
import filter from './filter';


export default combineReducers({
  config,
  rates,
  filter,
});
