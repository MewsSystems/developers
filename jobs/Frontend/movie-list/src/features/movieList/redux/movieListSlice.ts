import { createSlice, PayloadAction } from "@reduxjs/toolkit";

import { AppState } from "~/app/store";

const QUERY_INITIAL_STATE = "";
const PAGINATION_INITIAL_STATE = 1;

const initialState = {
  query: QUERY_INITIAL_STATE,
  pagination: PAGINATION_INITIAL_STATE,
};

export const movieListSlice = createSlice({
  name: "movieList",
  initialState,
  reducers: {
    changeSearchQuery: (state, action: PayloadAction<string>) => {
      state.query = action.payload;
    },
    resetQuery: (state) => {
      state.query = QUERY_INITIAL_STATE;
    },
    changePagination: (state, action: PayloadAction<number>) => {
      state.pagination = action.payload;
    },
    resetPagination: (state) => {
      state.pagination = PAGINATION_INITIAL_STATE;
    },
    resetState: (state) => {
      state = initialState
    }
  }
});

export const {changePagination, changeSearchQuery, resetPagination } = movieListSlice.actions

export const selectQuery = (state: AppState) => state.movieList.query;

export const selectPagination = (state: AppState) => state.movieList.pagination;

export default movieListSlice.reducer;