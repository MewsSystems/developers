import { createStore } from 'redux';
import rootReducer from './reducers';

// set initial state
const initialState = {
  rates: {},
  pairs: {},
};

export default createStore(rootReducer, initialState);
