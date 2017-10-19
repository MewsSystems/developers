import {combineReducers} from 'redux';
import config from './config';
import rates from './rates';


export default combineReducers({
  config,
  rates,
});
