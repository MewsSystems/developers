import {
	fetchMovies,
	fetchMovieDetails,
	API_ENDPOINTS,
} from "../services/movieService";
import apiClient from "../services/apiClient";

// Mock the apiClient module
jest.mock("../services/apiClient", () => ({
	get: jest.fn(),
}));

describe("movieService", () => {
	afterEach(() => {
		jest.clearAllMocks();
	});

	describe("fetchMovies", () => {
		it("should call the correct API endpoint with the right parameters", async () => {
			const mockResponse = { data: { results: [] } };
			const query = "Inception";
			const page = 1;

			// Mock the response for apiClient.get
			(apiClient.get as jest.Mock).mockResolvedValue(mockResponse);

			const response = await fetchMovies(query, page);

			// Assert the correct API endpoint and params
			expect(apiClient.get).toHaveBeenCalledWith(API_ENDPOINTS.SEARCH_MOVIES, {
				params: { query, page },
			});

			// Check the returned data
			expect(response).toEqual(mockResponse.data);
		});

		it("should throw an error when the request fails", async () => {
			const query = "Inception";
			const page = 1;

			// Mock a failed API request
			(apiClient.get as jest.Mock).mockRejectedValue(new Error("API Error"));

			try {
				await fetchMovies(query, page);
			} catch (error: any) {
				expect(error.message).toBe("Error: API Error");
			}
		});
	});

	describe("fetchMovieDetails", () => {
		it("should call the correct API endpoint with the right parameters", async () => {
			const mockResponse = { data: { id: 123, title: "Inception" } };
			const movieId = "123";

			// Mock the response for apiClient.get
			(apiClient.get as jest.Mock).mockResolvedValue(mockResponse);

			const response = await fetchMovieDetails(movieId);

			// Assert the correct API endpoint
			expect(apiClient.get).toHaveBeenCalledWith(
				API_ENDPOINTS.MOVIE_DETAILS(movieId)
			);

			// Check the returned data
			expect(response).toEqual(mockResponse.data);
		});

		it("should throw an error when the request fails", async () => {
			const movieId = "123";

			// Mock a failed API request
			(apiClient.get as jest.Mock).mockRejectedValue(new Error("API Error"));

			try {
				await fetchMovieDetails(movieId);
			} catch (error: any) {
				expect(error.message).toBe("Error: API Error");
			}
		});
	});
});
