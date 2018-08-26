import {combineReducers} from 'redux';
import config from './configReducer';

const rootReducer = combineReducers({
  config
});

export default rootReducer;