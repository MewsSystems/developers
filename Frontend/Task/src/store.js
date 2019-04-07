import { createStore, compose, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import rootReducer from './reducers';
import immediateRatesFetcher from './middlewares/immediate-rates-fetcher';
import { loadState, saveState } from './helpers/local-storage';

/* eslint-disable no-underscore-dangle */
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
/* eslint-enable */

const persistedState = loadState();

const store = createStore(
  rootReducer,
  persistedState,
  composeEnhancers(
    applyMiddleware(
      thunk,
      immediateRatesFetcher,
    ),
  ),
);

store.subscribe(() => {
  saveState(store.getState());
});

if (module.hot) {
  // Enable Webpack hot module replacement for reducers
  module.hot.accept('./reducers', () => {
    store.replaceReducer(rootReducer);
  });
}

export default store;
