import { describe, it, expect } from 'vitest';
import { getMoviesAdapter } from '@core/movie/services/adapter/get-movies-adapter';
import { mockMoviesListResponseByTMDB } from '@test/mocks/movie/movies-list-mock';

describe('getMoviesAdapter', () => {
  it('should transform array of API responses to Movie array', () => {
    const result = getMoviesAdapter(mockMoviesListResponseByTMDB.results);

    expect(result).toEqual([
      {
        id: 123,
        title: 'Test Movie 1',
        overview: 'Test overview 1',
        posterPath: '/test-poster-1.jpg',
        releaseDate: '2024-01-01',
        voteAverage: 8.5,
        voteCount: 1000,
        popularity: 100,
        backdropPath: '/test-backdrop-1.jpg',
        language: 'en',
        video: false,
        runtime: 120
      },
      {
        id: 456,
        title: 'Test Movie 2',
        overview: 'Test overview 2',
        posterPath: '/test-poster-2.jpg',
        releaseDate: '2024-01-02',
        voteAverage: 7.5,
        voteCount: 500,
        popularity: 50,
        backdropPath: '/test-backdrop-2.jpg',
        language: 'es',
        video: false,
        runtime: 90
      }
    ]);
  });

  it('should handle empty array', () => {
    const result = getMoviesAdapter([]);
    expect(result).toEqual([]);
  });

  it('should handle missing optional fields', () => {
    const partialResponse = [{
      ...mockMoviesListResponseByTMDB.results[0],
      overview: undefined,
      poster_path: undefined,
      backdrop_path: undefined,
      runtime: undefined
    }];

    const result = getMoviesAdapter(partialResponse);

    expect(result).toEqual([{
      id: 123,
      title: 'Test Movie 1',
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
    }]);
  });

  it('should handle null values in movie objects', () => {
    const responseWithNulls = [{
      ...mockMoviesListResponseByTMDB.results[0],
      overview: null,
      poster_path: null,
      backdrop_path: null,
      runtime: null
    }];

    const result = getMoviesAdapter(responseWithNulls);

    expect(result).toEqual([{
      id: 123,
      title: 'Test Movie 1',
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
    }]);
  });

  it('should handle extra fields in the response', () => {
    const responseWithExtraFields = [{
      ...mockMoviesListResponseByTMDB.results[0],
      extra_field: 'extra value',
    }];

    const result = getMoviesAdapter(responseWithExtraFields);

    expect(result).toEqual([{
      id: 123,
      title: 'Test Movie 1',
      overview: 'Test overview 1',
      posterPath: '/test-poster-1.jpg',
      releaseDate: '2024-01-01',
      voteAverage: 8.5,
      voteCount: 1000,
      popularity: 100,
      backdropPath: '/test-backdrop-1.jpg',
      language: 'en',
      video: false,
      runtime: 120
    }]);
  });

  it('should handle minimal valid data', () => {
    const minimalResponse = [{
      ...mockMoviesListResponseByTMDB.results[0],
      overview: undefined,
      poster_path: undefined,
      release_date: undefined,
      vote_average: undefined,
      vote_count: undefined,
      popularity: undefined,
      backdrop_path: undefined,
      runtime: undefined
    }];

    const result = getMoviesAdapter(minimalResponse);

    expect(result).toEqual([{
      id: 123,
      title: 'Test Movie 1',
      overview: null,
      posterPath: null,
      releaseDate: null,
      voteAverage: null,
      voteCount: null,
      popularity: null,
      backdropPath: null,
      language: 'en',
      video: false,
      runtime: null
    }]);
  });
}); 