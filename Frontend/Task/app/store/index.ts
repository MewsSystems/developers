import {applyMiddleware, createStore} from 'redux';
import thunk from 'redux-thunk';
import rootReducer from '../reducers/index';
import {initialStore} from "../common/constants";

export default createStore(rootReducer, initialStore, applyMiddleware(thunk));