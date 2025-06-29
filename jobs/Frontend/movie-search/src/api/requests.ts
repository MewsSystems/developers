import axios from "axios";
import type { MovieDetailsResponse, PopularMoviesResponse } from "./types";

export const BASE_URL = "https://api.themoviedb.org/3";
const API_KEY: string = import.meta.env.VITE_TMDB_API_KEY;

const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    Authorization: `Bearer ${API_KEY}`,
    "Content-Type": "application/json",
  },
});

/**
 * Fetch list of popular movies from TMDb API
 * @param page - Page number to query (default = 1)
 */
export const getPopularMovies = async (
  page = 1
): Promise<PopularMoviesResponse> => {
  try {
    const response = await api.get<PopularMoviesResponse>("/movie/popular", {
      params: { page },
    });
    return response.data;
  } catch (error) {
    // TBD Handle error
    console.error(error);
    throw error;
  }
};

/**
 * Fetch detailed information for a single movie by ID
 * @param movieId - The TMDb ID of the movie
 */
export const getMovieDetails = async (
  movieId: number
): Promise<MovieDetailsResponse> => {
  try {
    const response = await api.get<MovieDetailsResponse>(`/movie/${movieId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching movie details", error);
    throw error;
  }
};

/**
 * Fetch detailed information for a single movie by ID
 * @param query - Query which will be used to call TMDb API
 * @param page - Page number to query (default = 1)
 */
export const getSearchMovies = async (
  query: string,
  page: number
): Promise<PopularMoviesResponse> => {
  try {
    const response = await api.get<PopularMoviesResponse>("/search/movie", {
      params: { query, page },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching search results", error);
    throw error;
  }
};
