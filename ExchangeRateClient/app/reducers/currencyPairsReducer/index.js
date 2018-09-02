import { combineReducers } from 'redux';
import allIds from './allIds';
import byId from './byId';

export default combineReducers({
    allIds,
    byId,
});
