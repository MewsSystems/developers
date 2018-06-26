import { createStore, applyMiddleware } from 'redux';
import logger from 'redux-logger';
import appReducer from './modules/reducer';

const initialState = {

};

export default () => createStore(appReducer, initialState, applyMiddleware(logger));
