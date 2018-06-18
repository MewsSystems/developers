import {createStore, applyMiddleware} from 'redux';
import logger from 'redux-logger';
import thunk from 'redux-thunk';
// import appReducer from './modules/reducer';
import reducer from './modules/rates/reducer';

export default createStore(reducer, applyMiddleware(logger, thunk));
