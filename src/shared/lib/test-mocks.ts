/**
 * Mock data for testing
 */
import type { Movie, MovieResponse } from '@/entities/movie';

// Mock movies data
export const mockMovies: Array<Movie> = [
  {
    id: 1,
    title: 'Test Movie 1',
    overview: 'This is a test movie overview for the first movie',
    poster_path: '/test-poster-1.jpg',
    backdrop_path: '/test-backdrop-1.jpg',
    release_date: '2023-01-01',
    vote_average: 8.5,
    vote_count: 1000,
    genre_ids: [28, 12],
    adult: false,
    original_language: 'en',
    original_title: 'Test Movie 1',
    popularity: 100.5,
    video: false,
  },
  {
    id: 2,
    title: 'Test Movie 2',
    overview: 'This is a test movie overview for the second movie',
    poster_path: '/test-poster-2.jpg',
    backdrop_path: '/test-backdrop-2.jpg',
    release_date: '2023-02-01',
    vote_average: 7.8,
    vote_count: 800,
    genre_ids: [35, 18],
    adult: false,
    original_language: 'en',
    original_title: 'Test Movie 2',
    popularity: 85.3,
    video: false,
  },
  {
    id: 3,
    title: 'Test Movie 3',
    overview: 'This is a test movie overview for the third movie',
    poster_path: '/test-poster-3.jpg',
    backdrop_path: '/test-backdrop-3.jpg',
    release_date: '2023-03-01',
    vote_average: 9.2,
    vote_count: 1500,
    genre_ids: [16, 10751],
    adult: false,
    original_language: 'en',
    original_title: 'Test Movie 3',
    popularity: 120.7,
    video: false,
  },
];

// Mock movie response with pagination
export const mockMovieResponse: MovieResponse = {
  page: 1,
  results: mockMovies,
  total_pages: 10,
  total_results: 200,
};

// Mock empty response
export const mockEmptyResponse: MovieResponse = {
  page: 1,
  results: [],
  total_pages: 0,
  total_results: 0,
};