import { configureStore } from "@reduxjs/toolkit";
import moviesReducer from "./movies/slice";
import modalReducer from "./modal/slice";
import movieIdReducer from "./MovieId/slice";
export const store = configureStore({
  reducer: {
    moviesSearch: moviesReducer,
    modalState: modalReducer,
    movieId: movieIdReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
