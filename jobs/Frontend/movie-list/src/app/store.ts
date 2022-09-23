import { configureStore } from "@reduxjs/toolkit"
// ...

export const store = configureStore({
  reducer: {}
})

export type AppState = ReturnType<typeof store.getState>

export type AppDispatch = typeof store.dispatch