import { createSlice } from "@reduxjs/toolkit"

export const moviesSlice = createSlice({
  name: "movies",
  initialState: {},
  reducers: {
    save: (state, action) => {
      state += action.payload
    },
  },
})

export const { save } = moviesSlice.actions

export default moviesSlice.reducer
