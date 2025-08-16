import { combineReducers, configureStore } from '@reduxjs/toolkit';

import searchReducer from './reducers/searchReducer';
import resultsReducer from './reducers/resultsReducer';
import thunkMiddleware from './middlewares/thunkMiddleware';
import { throttle } from 'lodash';
import { loadState, saveState } from './utils/sessionStorage';
import detailReducer from './reducers/detailReducer';

const persistedState = loadState();

const store = configureStore({
  reducer: { search: searchReducer, results: resultsReducer, detail: detailReducer },
  preloadedState: persistedState,
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(thunkMiddleware),
});

store.subscribe(
  throttle(() => {
    const state = store.getState();
    saveState(state);
  }, 500),
);

export default store;
