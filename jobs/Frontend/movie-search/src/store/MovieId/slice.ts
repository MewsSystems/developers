import { PayloadAction, createSlice } from "@reduxjs/toolkit";

export const movieIdSlice = createSlice({
  name: "movieId",
  initialState: {
    id: -1,
  },
  reducers: {
    setMovieId: (state, action: PayloadAction<number>) => {
      state.id = action.payload;
    },
  },
});
export const { setMovieId } = movieIdSlice.actions;
export default movieIdSlice.reducer;
