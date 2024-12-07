import { configureStore } from "@reduxjs/toolkit";
import activeSearchEngineReducer from "./searchEngine";

export const store = configureStore({
  reducer: {
    searchEngine: activeSearchEngineReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
