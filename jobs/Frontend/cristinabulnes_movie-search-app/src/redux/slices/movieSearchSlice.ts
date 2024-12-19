import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Movie } from "../../types";
import { fetchMoviesAsync } from "../thunk/movieSearchThunk";

interface MoviesState {
	query: string;
	movies: Movie[];
	isLoading: boolean;
	error: string | null;
	hasMore: boolean;
	page: number;
}

const initialState: MoviesState = {
	query: "",
	movies: [],
	isLoading: false,
	error: null,
	hasMore: false,
	page: 1,
};

const moviesSlice = createSlice({
	name: "movies",
	initialState,
	reducers: {
		setQuery(state, action: PayloadAction<string>) {
			state.query = action.payload;
			state.page = 1;
		},
		loadMore(state) {
			state.page += 1;
		},
	},
	extraReducers: (builder) => {
		builder
			.addCase(fetchMoviesAsync.pending, (state) => {
				state.isLoading = true;
				state.error = null;
			})
			.addCase(fetchMoviesAsync.fulfilled, (state, action) => {
				const { results, page, total_pages } = action.payload;
				state.isLoading = false;
				state.movies = page === 1 ? results : [...state.movies, ...results];
				state.hasMore = page < total_pages;
			})
			.addCase(fetchMoviesAsync.rejected, (state) => {
				state.isLoading = false;
				state.error = "Failed to load movies.";
			});
	},
});

export const { setQuery, loadMore } = moviesSlice.actions;
export default moviesSlice.reducer;
