import { configureStore } from "@reduxjs/toolkit";
import { middleware } from "./middlewares";
import { rootReducer } from "./reducers";

export const store = configureStore({
  reducer: rootReducer,
  middleware,
  devTools: true,
});
