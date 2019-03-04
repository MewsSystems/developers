import { applyMiddleware, createStore } from 'redux';
import thunkMiddleware from 'redux-thunk';
import promise from 'redux-promise-middleware';

import rootReducer from './rootReducer';

export default function configureStore(preloadedState, config, env) {
  const middlewares = [
    thunkMiddleware.withExtraArgument({
      getConfig: () => config,
    }),
    promise,
  ];

  if (env.NODE_ENV !== 'production') {
    const logger = require('redux-logger'); // eslint-disable-line
    middlewares.push(logger.createLogger({ collapsed: true }));
  }

  return createStore(
    rootReducer,
    preloadedState,
    applyMiddleware(...middlewares),
  );
}
