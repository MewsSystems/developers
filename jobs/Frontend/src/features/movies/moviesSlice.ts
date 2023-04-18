import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { findMovies } from "../../common/api";
import type { MovieSearchResponse } from "../../common/movie";
import type { RootState } from "../../app/store";

export const search = createAsyncThunk("movies/search", ({ query, page }: { query: string; page: number }) => {
	return findMovies(query, page);
});

type MovieSearchState = MovieSearchResponse & {
	query: string;
	status: "Idle" | "Loading" | "Succeeded" | "Failed";
};

const initialState: MovieSearchState = {
	query: "",
	currentPage: 0,
	totalPages: 0,
	results: [],
	status: "Idle",
};

export const moviesSlice = createSlice({
	name: "movies",
	initialState,
	reducers: {
		queryChanged(state, action: PayloadAction<string>) {
			state.query = action.payload;
			state.currentPage = 0;
			state.totalPages = 0;
			state.results = [];
		},
		pageChanged(state, action: PayloadAction<number>) {
			state.currentPage = action.payload;
		},
	},
	extraReducers(builder) {
		builder
			.addCase(search.pending, (state) => {
				state.status = "Loading";
			})
			.addCase(search.fulfilled, (state, action) => {
				state.currentPage = action.payload.currentPage;
				state.totalPages = action.payload.totalPages;
				state.results = action.payload.results;
				state.status = "Succeeded";
			})
			.addCase(search.rejected, (state) => {
				state.results = [];
				state.status = "Failed";
			});
	},
});

export const { queryChanged, pageChanged } = moviesSlice.actions;

export const selectResults = (state: RootState) => state.movies.results;
export const selectQuery = (state: RootState) => state.movies.query;
export const selectCurrentPage = (state: RootState) => state.movies.currentPage;
export const selectTotalPages = (state: RootState) => state.movies.totalPages;
export const selectStatus = (state: RootState) => state.movies.status;

export default moviesSlice.reducer;
