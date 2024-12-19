import { createAsyncThunk } from "@reduxjs/toolkit";
import { fetchMovies, SearchMoviesResponse } from "../../services/movieService";

export const fetchMoviesAsync = createAsyncThunk<
	SearchMoviesResponse,
	{ query: string; page: number }
>(
	"movies/fetchMovies",
	async ({ query, page }: { query: string; page: number }) => {
		const response = await fetchMovies(query, page);
		return response;
	}
);
