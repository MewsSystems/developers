import {createStore, applyMiddleware } from 'redux';
import moviesReducer from "./moviesReducer";
import thunk from 'redux-thunk';
const middleware = applyMiddleware(thunk)
export default createStore(moviesReducer,middleware);
