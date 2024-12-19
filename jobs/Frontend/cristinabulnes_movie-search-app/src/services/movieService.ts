import { Movie } from "../types";
import apiClient from "./apiClient";
import {
	mapFetchMoviesResponse,
	mapFetchMovieDetailsResponse,
	MovieDetailsApiResponse,
	PaginatedSearchMoviesApiResponse,
} from "../utils/api/responseMappers";

export const API_ENDPOINTS = {
	SEARCH_MOVIES: "/search/movie",
	MOVIE_DETAILS: (id: string) => `/movie/${id}`,
};

export interface SearchMoviesResponse {
	results: Movie[];
	page: number;
	total_pages: number;
	total_results: number;
}

// Fetch movies by search query
export const fetchMovies = async (
	query: string,
	page: number
): Promise<SearchMoviesResponse> => {
	try {
		const response = await apiClient.get<PaginatedSearchMoviesApiResponse>(
			API_ENDPOINTS.SEARCH_MOVIES,
			{ params: { query, page } }
		);

		// Map results and maintain other pagination fields
		const transformedMovies = response.data.results.map(mapFetchMoviesResponse);
		return {
			...response.data,
			results: transformedMovies,
		};
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
