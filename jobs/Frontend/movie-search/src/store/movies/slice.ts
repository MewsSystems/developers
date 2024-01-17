import { createSlice } from "@reduxjs/toolkit";

export const moviesSlice = createSlice({
  name: "moviesSearch",
  initialState: {
    userSearch: "",
  },
  reducers: {
    setUserSearch: (state, action) => {
      state.userSearch = action.payload;
    },
  },
});

export const { setUserSearch } = moviesSlice.actions;
export default moviesSlice.reducer;
