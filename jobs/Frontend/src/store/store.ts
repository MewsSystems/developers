import {
  configureStore,
  type ThunkAction,
  type Action, combineReducers,
} from '@reduxjs/toolkit';
import {
  useSelector as useReduxSelector,
  useDispatch as useReduxDispatch,
  type TypedUseSelectorHook,
} from "react-redux";
import { reducer } from "./rootReducer";
import { middleware } from "./middleware";
import { TMDBApiSlice } from '@/store/slices';

export const setupStore = (preloadedState?: Partial<ReduxState>) => {
  return configureStore({
    reducer: combineReducers(reducer),
    preloadedState,
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(TMDBApiSlice.middleware),
  })
}

export const reduxStore = configureStore({
  reducer: combineReducers(reducer),
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(middleware, TMDBApiSlice.middleware),
});
export const useDispatch = () => useReduxDispatch<ReduxDispatch>();
export const useSelector: TypedUseSelectorHook<ReduxState> = useReduxSelector;

/* Types */
export type ReduxStore = typeof reduxStore;
export type ReduxState = ReturnType<typeof reduxStore.getState>;
export type ReduxDispatch = typeof reduxStore.dispatch;
export type ReduxThunkAction<ReturnType = void> = ThunkAction<
  ReturnType,
  ReduxState,
  unknown,
  Action
>;
