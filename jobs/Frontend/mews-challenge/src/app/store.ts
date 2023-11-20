import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import appReducer from './services/appReducer';
import { movieApi } from './services/movie';

export const store = configureStore({
  reducer: {
    app: appReducer,
    [movieApi.reducerPath]: movieApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(movieApi.middleware),
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
