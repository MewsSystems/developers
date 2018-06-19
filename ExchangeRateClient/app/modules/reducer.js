import {combineReducers} from 'redux';
import {reducer as ratesReducer, MODULE_NAME as ratesKey} from './rates';

export default combineReducers({
    [ratesKey]: ratesReducer
});
