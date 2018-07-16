import { combineReducers } from 'redux';
import dialogReducer from './dialogReducer';

export default combineReducers({
	dialogReducer: dialogReducer
	// ...another future reducers
});
