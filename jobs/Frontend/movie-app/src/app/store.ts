import { configureStore, ThunkAction, Action } from "@reduxjs/toolkit"
import moviesReducer from "@/features/movies/moviesSlice"
import { apiSlice } from "../features/api/apiSlice"

export const store = configureStore({
  reducer: {
    movies: moviesReducer,
    [apiSlice.reducerPath]: apiSlice.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(apiSlice.middleware),
})

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof store.getState>
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>
