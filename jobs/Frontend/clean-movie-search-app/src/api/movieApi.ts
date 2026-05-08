import { Movie, MovieSearchResponse } from './types';

// This could go to the environment variables
// But for the sake of simplicity and it being posted elsewhere in the repo, I'm keeping it here
const API_KEY = '03b8572954325680265531140190fd2a';
const BASE_URL = 'https://api.themoviedb.org/3';

/**
 * Fetches a URL with a timeout mechanism.
 * If the request takes longer than the specified timeout, it is aborted.
 */
const fetchWithTimeout = async (
  url: string,
  options: RequestInit = {},
  timeout = 5000
): Promise<Response> => {
  const controller = new AbortController();
  const timer = setTimeout(() => controller.abort(), timeout);

  try {
    const response = await fetch(url, {
      ...options,
      signal: controller.signal,
    });
    return response;
  } finally {
    clearTimeout(timer);
  }
};

/**
 * Makes a fetch request with retries for transient errors (HTTP 500-504).
 * Uses exponential backoff to delay retries.
 */
const retryFetch = async (
  url: string,
  options: RequestInit = {},
  retries = 3,
  delay = 500
): Promise<Response> => {
  for (let i = 0; i <= retries; i++) {
    try {
      const response = await fetchWithTimeout(url, options);
      if (!response.ok) {
        // Throw error for non-retryable status or after retries
        if (i === retries || ![500, 502, 503, 504].includes(response.status)) {
          throw new Error(`HTTP Error: ${response.status}`);
        }
      } else {
        return response; // Return successful response
      }
    } catch (error) {
      if (i === retries) {
        throw error; // Throw final error after max retries
      }
      await new Promise((resolve) => setTimeout(resolve, delay * 2 ** i)); // Exponential backoff
    }
  }
  throw new Error('Max retries reached'); // Fallback
};

/**
 * Validates if the given data matches the Movie type.
 */
const isMovie = (data: any): data is Movie => {
  return (
    data &&
    typeof data.id === 'number' &&
    typeof data.title === 'string' &&
    typeof data.overview === 'string' &&
    (typeof data.poster_path === 'string' || data.poster_path === null) &&
    typeof data.release_date === 'string' &&
    typeof data.vote_average === 'number'
  );
};

/**
 * Validates if the given data matches the MovieSearchResponse type.
 */
const isMovieSearchResponse = (data: any): data is MovieSearchResponse => {
  return (
    data &&
    typeof data.page === 'number' &&
    Array.isArray(data.results) &&
    data.results.every(isMovie) &&
    typeof data.total_pages === 'number' &&
    typeof data.total_results === 'number'
  );
};

/**
 * Validates the response data against the provided validator.
 * Logs the data and throws an error if validation fails.
 */
const validateResponse = <T>(
  data: any,
  validator: (data: any) => data is T
): T => {
  if (!validator(data)) {
    console.error('Invalid response format', { data });
    throw new Error('Invalid response format');
  }
  return data;
};

/**
 * Searches for movies matching the provided query and page number.
 * Implements a delay to simulate rate-limiting and validates the response.
 */
export const searchMovies = async (
  query: string,
  page: number = 1
): Promise<MovieSearchResponse> => {
  // Introduce a delay of 0.5 second (500 milliseconds) - Kayak style
  await new Promise((resolve) => setTimeout(resolve, 500));
  const response = await retryFetch(
    `${BASE_URL}/search/movie?api_key=${API_KEY}&query=${encodeURIComponent(query)}&page=${page}`
  );

  const jsonData = await response.json();
  return validateResponse<MovieSearchResponse>(jsonData, isMovieSearchResponse);
};

/**
 * Retrieves detailed information about a movie by its ID.
 * Validates the response against the Movie type.
 */
export const getMovieDetails = async (movieId: number): Promise<Movie> => {
  const response = await retryFetch(
    `${BASE_URL}/movie/${movieId}?api_key=${API_KEY}`
  );

  const jsonData = await response.json();
  return validateResponse<Movie>(jsonData, isMovie);
};
