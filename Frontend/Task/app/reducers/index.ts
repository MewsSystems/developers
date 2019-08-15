import {combineReducers} from "redux";
import app from '../reducers/app';
import user from '../reducers/user';

export default combineReducers({app, user});
