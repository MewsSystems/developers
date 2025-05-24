import { describe, it, expect } from 'vitest';
import { mockMovieResponseByTMDB } from '@test/mocks/movie/movie-mock';
import { getMovieDetailsAdapter } from '@core/movie/services/adapter/get-movie-adapter';

describe('getMovieDetailsAdapter', () => {
  it('should transform API response to Movie type', () => {
    const result = getMovieDetailsAdapter(mockMovieResponseByTMDB);

    expect(result).toEqual({
      id: 123,
      title: 'Test Movie',
      overview: 'Test overview',
      posterPath: '/test-poster.jpg',
      releaseDate: '2024-01-01',
      voteAverage: 8.5,
      voteCount: 1000,
      popularity: 100,
      backdropPath: '/test-backdrop.jpg',
      language: 'en',
      video: false,
      runtime: 120
    });
  });

  it('should handle null values in response', () => {
    const responseWithNulls = {
      ...mockMovieResponseByTMDB,
      overview: null,
      poster_path: null,
      backdrop_path: null,
      runtime: null
    };

    const result = getMovieDetailsAdapter(responseWithNulls);

    expect(result).toEqual({
      id: 123,
      title: 'Test Movie',
      overview: null,
      posterPath: null,
      releaseDate: '2024-01-01',
      voteAverage: 8.5,
      voteCount: 1000,
      popularity: 100,
      backdropPath: null,
      language: 'en',
      video: false,
      runtime: null
    });
  });

  it('should handle zero values for numeric fields', () => {
    const responseWithZeros = {
      ...mockMovieResponseByTMDB,
      vote_average: 0,
      vote_count: 0,
      popularity: 0,
      runtime: 0
    };

    const result = getMovieDetailsAdapter(responseWithZeros);

    expect(result).toEqual({
      id: 123,
      title: 'Test Movie',
      overview: 'Test overview',
      posterPath: '/test-poster.jpg',
      releaseDate: '2024-01-01',
      voteAverage: 0,
      voteCount: 0,
      popularity: 0,
      backdropPath: '/test-backdrop.jpg',
      language: 'en',
      video: false,
      runtime: 0
    });
  });
}); 