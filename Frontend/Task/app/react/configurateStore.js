import { createPromise } from 'redux-promise-middleware';
import thunk from 'redux-thunk';
import { createStore, applyMiddleware } from 'redux';

import appReducer from './reducers';

export default function configureStore({ initialState }) {
  const logDeps = [thunk];
  
  if (process.env.NODE_ENV !== 'production') {
    const logger = require('redux-logger').createLogger();
    logDeps.push(logger);
  }
  
  const middleware = [
    createPromise({
      promiseTypeSuffixes: ['START', 'SUCCESS', 'ERROR']
    }),
    ...logDeps
];
  
  const createReduxStore = applyMiddleware(...middleware);
  
  const store = createReduxStore(createStore)(appReducer, initialState);
  
  if (module.hot) {
    module.hot.accept('./reducers/index', () => {
      const nextRootReducer = require('./reducers/index').default;
    store.replaceReducer(nextRootReducer);
  });
  }
  
  return store;
}
