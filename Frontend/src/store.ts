import thunk from 'redux-thunk';
import { createStore, applyMiddleware } from 'redux';
import logger from 'redux-logger';
import { createPromise } from 'redux-promise-middleware';

import reducer from './reducers';

const configureStore = () => {
  const middleware = [
    thunk,
    createPromise({
      promiseTypeSuffixes: ['START', 'SUCCESS', 'ERROR'],
    }),
  ];

  if (process.env.NODE_ENV !== 'production') {
    middleware.push(logger);
  }

  return createStore(reducer, applyMiddleware(...middleware));
};

export default configureStore;
