import { renderHook, act } from "@testing-library/react-hooks";
import { useMovieSearch } from "./useMovieSearch";
import { fetchMovies } from "../services/movieService";
import { mockMovies } from "../__mocks__/mockMovies";

// Mock the service that fetches the movies
jest.mock("../services/movieService");

describe("useMoviesSearch", () => {
	beforeEach(() => {
		(fetchMovies as jest.Mock).mockResolvedValue({
			results: mockMovies,
			page: 1,
			total_pages: 2,
		});
	});

	afterEach(() => {
		jest.clearAllMocks(); // Clean up mocks after each test
	});

	it("should fetch and set movies on query change after debounce", async () => {
		const { result, waitFor } = renderHook(() => useMovieSearch());

		// Act: Set the query to trigger the debounce
		act(() => {
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout (500ms)
		await new Promise((resolve) => setTimeout(resolve, 500));

		// Wait for isLoading to become false after the fetch completes
		await waitFor(() => result.current.isLoading === false);

		// Assert that `fetchMovies` was called with the correct query and page number
		expect(fetchMovies).toHaveBeenCalledWith("Batman", 1);

		// Assert the resulting state
		expect(result.current.movies).toEqual(mockMovies);
		expect(result.current.isLoading).toBe(false);
		expect(result.current.error).toBeNull();
		expect(result.current.hasMore).toBe(true);
	});

	it("should set error when fetch fails", async () => {
		// Simulate a failed fetch by rejecting the promise
		(fetchMovies as jest.Mock).mockRejectedValue(new Error("Network Error"));

		const { result, waitFor } = renderHook(() => useMovieSearch());

		// Act: Set the query to trigger the debounce
		act(() => {
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout (500ms)
		await new Promise((resolve) => setTimeout(resolve, 500));

		// Wait for isLoading to become false after the fetch failure
		await waitFor(() => result.current.isLoading === false);

		// Assert that the error state is updated
		expect(result.current.error).toBe("Failed to load movies.");
		expect(result.current.isLoading).toBe(false);
		expect(result.current.movies).toEqual([]); // No movies should be set
	});
});
