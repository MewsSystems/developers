import { configureStore } from "@reduxjs/toolkit";
import moviesReducer from "./movies/movies.slice";

export function setupMockStore(preloadedState?: RootState) {
  return configureStore({
    reducer: {
      movies: moviesReducer,
    },
    preloadedState: preloadedState || undefined,
  });
}

export const store = configureStore({
  reducer: {
    movies: moviesReducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;
