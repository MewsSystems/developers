import { configureStore } from "@reduxjs/toolkit";
import { moviesReducer } from "./containers/Movies/reducer";
import { useDispatch } from "react-redux";

export const store = configureStore({
  reducer: moviesReducer.reducer,
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export const useAppDispatch: () => AppDispatch = useDispatch;
