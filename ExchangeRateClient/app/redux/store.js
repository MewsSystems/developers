import { createStore } from 'redux';
import rootReducer from './reducers';

// set initial state
const initialState = {
  rates: {},
  pairs: {},
  trends: {},
};

export default createStore(
  rootReducer,
  initialState /* eslint-disable-next-line */,
  window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__(),
);
