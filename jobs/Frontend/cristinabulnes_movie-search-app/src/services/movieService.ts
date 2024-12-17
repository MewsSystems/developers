import apiClient from "./apiClient";

export const API_ENDPOINTS = {
	SEARCH_MOVIES: "/search/movie",
	MOVIE_DETAILS: (id: string) => `/movie/${id}`,
};

// Fetch movies by search query
export const fetchMovies = async (query: string, page: number) => {
	try {
		const response = await apiClient.get(API_ENDPOINTS.SEARCH_MOVIES, {
			params: { query, page },
		});
		return response.data;
	} catch (error: any) {
		console.error("Failed to fetch movies:", error);
		throw new Error(error || "Failed to fetch movies. Please try again.");
	}
};

// Fetch movie details by ID
export const fetchMovieDetails = async (movieId: string) => {
	try {
		const response = await apiClient.get(API_ENDPOINTS.MOVIE_DETAILS(movieId));
		return response.data;
	} catch (error: any) {
		console.error("Failed to fetch movie details:", error);
		throw new Error(
			error || "Failed to fetch movie details. Please try again."
		);
	}
};
