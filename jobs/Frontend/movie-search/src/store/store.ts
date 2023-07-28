import { configureStore } from "@reduxjs/toolkit";
// eslint-disable-next-line import/named
import { useDispatch, useSelector, TypedUseSelectorHook } from "react-redux";

import { ConfigSlice } from "./config-slice";
import { MoviesSlice } from "./movie-slice";

export const store = configureStore({
  reducer: {
    movies: MoviesSlice.reducer,
    conguration: ConfigSlice.reducer,
  },
});

export const useAppDispatch: () => typeof store.dispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<
  ReturnType<typeof store.getState>
> = useSelector;
