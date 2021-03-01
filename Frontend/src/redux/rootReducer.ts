import { combineReducers } from 'redux';
import searchReducer, { NAME as searchKey } from './searchReducer';
import movieReducer, { NAME as movieKey } from './movieReducer';
import configurationReducer, {
  NAME as configKey,
} from './configurationReducer';

const rootReducer = combineReducers({
  [searchKey]: searchReducer,
  [movieKey]: movieReducer,
  [configKey]: configurationReducer,
});

export default rootReducer;
