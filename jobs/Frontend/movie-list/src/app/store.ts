import { configureStore } from "@reduxjs/toolkit"
import { moviesApi } from "./rtkQuery"

import movieListReducer from "~/features/movieList/redux/movieListSlice"

export const store = configureStore({
  reducer: {
    [moviesApi.reducerPath]: moviesApi.reducer,
    movieList: movieListReducer
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(moviesApi.middleware),
})

export type AppState = ReturnType<typeof store.getState>

export type AppDispatch = typeof store.dispatch