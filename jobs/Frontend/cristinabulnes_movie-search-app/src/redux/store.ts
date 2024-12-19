import { configureStore } from "@reduxjs/toolkit";
import movieSearchReducer from "./slices/movieSearchSlice";

const store = configureStore({
	reducer: {
		movieSearch: movieSearchReducer,
	},
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
