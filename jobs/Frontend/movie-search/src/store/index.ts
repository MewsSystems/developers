import { configureStore } from "@reduxjs/toolkit";
import moviesReducer from "./movies/slice";

export const store = configureStore({
  reducer: {
    moviesSearch: moviesReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
