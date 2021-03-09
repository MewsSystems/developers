import { configureStore, Action } from '@reduxjs/toolkit';
import { ThunkAction } from 'redux-thunk';
import rootReducer from './redux/rootReducer';

const isDevelopmentEnv = process.env.NODE_ENV === 'development';

const store = configureStore({
  reducer: rootReducer,
  devTools: isDevelopmentEnv,
});

if (isDevelopmentEnv && module.hot) {
  module.hot.accept('./redux/rootReducer', () => {
    store.replaceReducer(rootReducer);
  });
}

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export type AppSelector<ReturnType, T extends any[] = []> = (
  state: RootState,
  ...args: T
) => ReturnType;

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;

export default store;
