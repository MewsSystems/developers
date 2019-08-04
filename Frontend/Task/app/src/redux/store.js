// @flow strict

import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';

import { INITIAL_STATE } from './constants';
import rootReducer from './reducers';

const middlewares = [thunk];

const store = createStore(rootReducer, INITIAL_STATE, applyMiddleware(...middlewares));
export default store;
