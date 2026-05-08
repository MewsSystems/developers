import { renderHook } from "@testing-library/react-hooks";
import { fetchMovieDetails } from "../services/movieService";
import { useMovieDetails } from "./useMovieDetails";
import { mockMovie } from "../__mocks__/mockMovies";

// Mocking the fetchMovieDetails service
jest.mock("../services/movieService");

describe("useMovieDetails", () => {
	it("should return movie details when fetch is successful", async () => {
		// Mocking the service to resolve with movie data
		(fetchMovieDetails as jest.Mock).mockResolvedValue(mockMovie);

		const { result, waitForNextUpdate } = renderHook(() =>
			useMovieDetails("1")
		);

		// Waiting for the hook to finish fetching
		await waitForNextUpdate();

		// Asserting that the hook returns the correct movie details
		expect(result.current.movieDetails).toEqual(mockMovie);
		expect(result.current.loading).toBe(false);
		expect(result.current.error).toBeNull();
	});

	it("should handle error when fetch fails", async () => {
		// Mocking the service to reject with an error
		(fetchMovieDetails as jest.Mock).mockRejectedValue(
			new Error("Network error")
		);

		const { result, waitForNextUpdate } = renderHook(() =>
			useMovieDetails("1")
		);

		// Waiting for the hook to finish fetching
		await waitForNextUpdate();

		// Asserting that an error is returned
		expect(result.current.movieDetails).toBeNull();
		expect(result.current.loading).toBe(false);
		expect(result.current.error).toBe(
			"Failed to load movie details. Please try again."
		);
	});

	it("should handle no movieId provided", async () => {
		const { result } = renderHook(() => useMovieDetails(undefined));

		// Asserting that the error message is returned when movieId is not provided
		expect(result.current.movieDetails).toBeNull();
		expect(result.current.loading).toBe(false);
		expect(result.current.error).toBe("No movie ID provided.");
	});
});
