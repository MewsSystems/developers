import { createStore, applyMiddleware, } from 'redux';
import thunkMiddleware from 'redux-thunk';

import rootReducer from './configureReducers';


/**
 * Configure Redux
 * @param {Object} initialState
 */
export const configureStore = (initialState) => {
  const create = window.devToolsExtension
    ? window.devToolsExtension()(createStore)
    : createStore;
  const createStoreWithMiddleware = applyMiddleware(thunkMiddleware)(create);
  const store = createStoreWithMiddleware(rootReducer, initialState);

  return store;
};
