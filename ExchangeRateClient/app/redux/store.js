import { createStore } from 'redux';
import rootReducer from './reducers';

// set initial state
const initialState = {
  rates: {},
};

export default createStore(rootReducer, initialState);
