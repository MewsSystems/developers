import {
  combineReducers,
  configureStore,
  EnhancedStore,
} from '@reduxjs/toolkit';
import movieReducer from './movies/movieSlice';

const rootReducer = combineReducers({
  movies: movieReducer,
});

export function setupStore(preloadedState?: Partial<RootState>): EnhancedStore {
  return configureStore({
    reducer: rootReducer,
    preloadedState,
  });
}

export const store = setupStore();

export type RootState = ReturnType<typeof store.getState>;
