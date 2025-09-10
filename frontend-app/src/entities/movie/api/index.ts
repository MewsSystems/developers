/**
 * Service for working with TMDB API
 */
import axios from 'axios';
import type { MovieDetailsType, MovieResponse } from '@/entities/movie';

const API_KEY = import.meta.env.VITE_TMDB_API_KEY || '';
const BASE_URL = 'https://api.themoviedb.org/3';

/**
 * Axios instance with TMDB API configuration
 */
const tmdbApi = axios.create({
  baseURL: BASE_URL,
  params: {
    api_key: API_KEY,
    language: 'en-US',
  },
  timeout: 10000,
});

/**
 * Search movies by query
 * @param query - search query
 * @param page - page number (default 1)
 * @param signal - AbortSignal for request cancellation
 */
export const searchMovies = async (
  query: string, 
  page: number = 1, 
  signal?: AbortSignal
): Promise<MovieResponse> => {
  try {
    const response = await tmdbApi.get<MovieResponse>('/search/movie', {
      params: {
        query,
        page,
      },
      signal,
    });
    return response.data;
  } catch (error) {
    console.error('Error searching movies:', error);
    throw error;
  }
};

/**
 * Get detailed movie information by ID
 * @param id - movie identifier
 * @param signal - AbortSignal for request cancellation
 */
export const getMovieDetails = async (
  id: number, 
): Promise<MovieDetailsType> => {
  try {
    const response = await tmdbApi.get<MovieDetailsType>(`/movie/${id}`, {
      params: {
        append_to_response: 'credits,videos,images',
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching movie details:', error);
    throw error;
  }
};

/**
 * Get popular movies
 * @param page - page number (default 1)
 * @param signal - AbortSignal for request cancellation
 */
export const getPopularMovies = async (
  page: number = 1, 
): Promise<MovieResponse> => {
  try {
    const response = await tmdbApi.get<MovieResponse>('/movie/popular', {
      params: {
        page,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching popular movies:', error);
    throw error;
  }
};