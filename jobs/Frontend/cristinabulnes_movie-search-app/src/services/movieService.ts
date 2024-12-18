import { Movie } from "../types";
import apiClient from "./apiClient";
import {
	mapFetchMoviesResponse,
	mapFetchMovieDetailsResponse,
	SearchMovieApiResponse,
	MovieDetailsApiResponse,
} from "../utils/api/responseMappers";

export const API_ENDPOINTS = {
	SEARCH_MOVIES: "/search/movie",
	MOVIE_DETAILS: (id: string) => `/movie/${id}`,
};

// Fetch movies by search query
export const fetchMovies = async (
	query: string,
	page: number
): Promise<{ results: Movie[] }> => {
	try {
		const response = await apiClient.get<{ results: SearchMovieApiResponse[] }>(
			API_ENDPOINTS.SEARCH_MOVIES,
			{ params: { query, page } }
		);

		// Use mapper to transform API response
		const transformedMovies = response.data.results.map(mapFetchMoviesResponse);
		return { results: transformedMovies };
	} catch (error: any) {
		console.error("Failed to fetch movies:", error);
		throw new Error(error || "Failed to fetch movies. Please try again.");
	}
};

// Fetch movie details by ID
export const fetchMovieDetails = async (movieId: string): Promise<Movie> => {
	try {
		const response = await apiClient.get<MovieDetailsApiResponse>(
			API_ENDPOINTS.MOVIE_DETAILS(movieId)
		);

		// Use mapper to transform API response
		const transformedMovie = mapFetchMovieDetailsResponse(response.data);
		return transformedMovie;
	} catch (error: any) {
		console.error("Failed to fetch movie details:", error);
		throw new Error(
			error || "Failed to fetch movie details. Please try again."
		);
	}
};
