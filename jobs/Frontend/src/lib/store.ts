import { configureStore } from '@reduxjs/toolkit'

import { apiSlice } from './features/api/apiSlice'
import filterSliceReducer from './features/filter/filterSlice'

/**
 * Because the Redux store is shared across requests, it should not be defined as a global variable.
 * Instead, the store should be created per request.
 *
 * @learn https://redux-toolkit.js.org/usage/nextjs#creating-a-redux-store-per-request
 */
export const makeStore = () => {
  return configureStore({
    reducer: {
      [apiSlice.reducerPath]: apiSlice.reducer,
      filter: filterSliceReducer,
    },
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat([apiSlice.middleware]),
  })
}

// Infer the type of makeStore
export type AppStore = ReturnType<typeof makeStore>
// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<AppStore['getState']>
export type AppDispatch = AppStore['dispatch']
