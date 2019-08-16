import {applyMiddleware, createStore} from 'redux';
import thunk from 'redux-thunk';
import cacheMiddleware from '../middlewares/cache';
import rootReducer from '../reducers/index';
import {initialStore} from "../common/constants";
import {injectCache} from "../common/helpers";

export default createStore(rootReducer, injectCache(initialStore), applyMiddleware(thunk, cacheMiddleware));