import { configureStore } from "@reduxjs/toolkit";
import { fetchMovies, SearchMoviesResponse } from "../../services/movieService";
import movieSearchReducer from "../slices/movieSearchSlice";
import { fetchMoviesAsync } from "./movieSearchThunk";

jest.mock("../../services/movieService");
const mockFetchMovies = fetchMovies as jest.MockedFunction<typeof fetchMovies>;

describe("fetchMoviesAsync", () => {
	let store: any;

	beforeEach(() => {
		store = configureStore({ reducer: { movieSearch: movieSearchReducer } });
	});

	it("should dispatch fulfilled action when movies are fetched successfully", async () => {
		const mockResponse: SearchMoviesResponse = {
			results: [],
			page: 1,
			total_pages: 1,
			total_results: 0,
		};
		mockFetchMovies.mockResolvedValue(mockResponse);

		await store.dispatch(fetchMoviesAsync({ query: "action", page: 1 }));

		const state = store.getState();
		expect(state.movieSearch.movies).toEqual(mockResponse.results);
		expect(state.movieSearch.isLoading).toBe(false);
		expect(state.movieSearch.hasMore).toBe(false);
	});

	it("should dispatch rejected action if fetching movies fails", async () => {
		mockFetchMovies.mockRejectedValue(new Error("Failed to load"));

		await store.dispatch(fetchMoviesAsync({ query: "action", page: 1 }));

		const state = store.getState();
		expect(state.movieSearch.error).toBe("Failed to load movies.");
		expect(state.movieSearch.isLoading).toBe(false);
	});
});
