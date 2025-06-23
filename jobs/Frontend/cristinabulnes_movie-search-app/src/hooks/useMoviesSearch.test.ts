import { renderHook, act } from "@testing-library/react-hooks";
import { useMovieSearch } from "./useMovieSearch";
import { fetchMovies } from "../services/movieService";
import { mockMovies } from "../__mocks__/mockMovies";
import { SearchMovieProvider } from "../contexts/SearchMovieContext";

// Mock the service that fetches the movies
jest.mock("../services/movieService");

describe("useMovieSearch", () => {
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

	// Utility function to render hook with the SearchMovieProvider
	const renderWithProvider = () => {
		return renderHook(() => useMovieSearch(), {
			wrapper: SearchMovieProvider, // Wrap the hook with the context provider
		});
	};

	it("should fetch and set movies on query change after debounce", async () => {
		const { result, waitFor } = renderWithProvider();

		// Act: Set the query to trigger the debounce
		act(() => {
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout
		await new Promise((resolve) => setTimeout(resolve, 600));

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

		const { result, waitFor } = renderWithProvider();

		// Act: Set the query to trigger the debounce
		act(() => {
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout
		await new Promise((resolve) => setTimeout(resolve, 600));

		// Wait for isLoading to become false after the fetch failure
		await waitFor(() => result.current.isLoading === false);

		// Assert that the error state is updated
		expect(result.current.error).toBe("Failed to load movies.");
		expect(result.current.isLoading).toBe(false);
		expect(result.current.movies).toEqual([]); // No movies should be set
	});

	it("should fetch and append movies when loadMore is called", async () => {
		const { result, waitFor } = renderWithProvider();

		// Act: Set the query and trigger initial fetch
		act(() => {
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout
		await new Promise((resolve) => setTimeout(resolve, 600));

		// Wait for initial fetch to complete
		await waitFor(() => result.current.isLoading === false);

		// Mock second page of results
		(fetchMovies as jest.Mock).mockResolvedValueOnce({
			results: mockMovies,
			page: 2,
			total_pages: 2,
		});

		// Act: Trigger loadMore
		act(() => {
			result.current.loadMore();
		});

		// Wait for the next fetch to complete
		await waitFor(() => result.current.isLoading === false);

		// Assert that the second fetch was triggered with the correct page
		expect(fetchMovies).toHaveBeenCalledWith("Batman", 2);

		// Assert that movies are appended
		expect(result.current.movies).toEqual([...mockMovies, ...mockMovies]);
		expect(result.current.hasMore).toBe(false); // No more pages
	});

	it("should clear movies and disable pagination when query is empty", async () => {
		const { result, waitFor } = renderWithProvider();

		// Act: Set a non-empty query
		act(() => {
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout
		await new Promise((resolve) => setTimeout(resolve, 600));

		// Wait for the initial fetch to complete
		await waitFor(() => result.current.isLoading === false);

		// Assert that movies are set
		expect(result.current.movies).toEqual(mockMovies);

		// Act: Set an empty query
		act(() => {
			result.current.setQuery("");
		});

		// Wait for the debounce timeout
		await new Promise((resolve) => setTimeout(resolve, 600));

		// Assert that movies are cleared and pagination is disabled
		expect(result.current.movies).toEqual([]);
		expect(result.current.hasMore).toBe(false);
	});
	it("should debounce and fetch only for the final query", async () => {
		const { result, waitFor } = renderWithProvider();

		// Simulate rapid query updates
		act(() => {
			result.current.setQuery("B");
			result.current.setQuery("Ba");
			result.current.setQuery("Bat");
			result.current.setQuery("Batm");
			result.current.setQuery("Batman");
		});

		// Wait for the debounce timeout
		await new Promise((resolve) => setTimeout(resolve, 600));

		// Wait for isLoading to become false
		await waitFor(() => result.current.isLoading === false);

		// Assert that `fetchMovies` was called only once with "Batman"
		expect(fetchMovies).toHaveBeenCalledTimes(1);
		expect(fetchMovies).toHaveBeenCalledWith("Batman", 1);
	});
});
