import { configureStore } from "@reduxjs/toolkit";
import { searchFormSlice } from "../features/search-form/slice";

export const store = configureStore({
    reducer: {
        search: searchFormSlice.reducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;