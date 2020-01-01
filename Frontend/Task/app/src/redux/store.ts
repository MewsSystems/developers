import { createStore, applyMiddleware } from 'redux';
import logger from 'redux-logger';
import thunk from 'redux-thunk';
import rootReducer from './root-reducer';
import {loadState} from '../utils'

const persistedState = loadState()

const middlewares = [thunk]

if (process.env.NODE_ENV === 'development') {
  middlewares.push(logger);
}

const store = createStore(rootReducer, persistedState, applyMiddleware(...middlewares));


export default store;