import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface reducerSearch {
  movieSearched: string;
}

const initialState: reducerSearch = {
  movieSearched: "",
};

export const searchSlice = createSlice({
  name: "currentQuery",
  initialState,
  reducers: {
    setMovieSearched: (state, action: PayloadAction<string>) => {
      state.movieSearched = action.payload;
    },
  },
});

export const { setMovieSearched } = searchSlice.actions;

export default searchSlice.reducer;
