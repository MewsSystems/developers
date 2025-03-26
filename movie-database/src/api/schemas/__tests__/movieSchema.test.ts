import { describe, it, expect } from 'vitest';
import { movieSchema, movieResultSchema, movieResultsListSchema } from '../movieSchema';
import fallbackImage from "@/assets/poster-fallback.webp";

describe('Movie schemas parsing and validations', () => {
  describe('movieResultSchema', () => {
    it('transforms movie result data correctly', () => {
      const inputData = {
        id: 1,
        backdrop_path: '/backdrop.jpg',
        poster_path: '/poster.jpg',
        release_date: '2023-05-15',
        title: 'Test Movie',
        overview: 'A long enough overview for testing',
        vote_average: 7.512,
        vote_count: 1000
      };

      const result = movieResultSchema.parse(inputData);

      expect(result).toEqual({
        id: 1,
        year: '2023',
        title: 'Test Movie',
        overview: 'A long enough overview for testing',
        voteAverage: '7.5',
        voteCount: 1000,
        image: 'https://image.tmdb.org/t/p/w400/backdrop.jpg'
      });
    });

    it('uses poster_path when backdrop_path is null', () => {
      const inputData = {
        id: 1,
        backdrop_path: null,
        poster_path: '/poster.jpg',
        release_date: '2023-05-15',
        title: 'Test Movie',
        overview: 'A long enough overview for testing',
        vote_average: 7.5,
        vote_count: 1000
      };

      const result = movieResultSchema.parse(inputData);

      expect(result.image).toBe('https://image.tmdb.org/t/p/w400/poster.jpg');
    });

    it('uses fallback image when both backdrop_path and poster_path are null', () => {
      const inputData = {
        id: 1,
        backdrop_path: null,
        poster_path: null,
        release_date: '2023-05-15',
        title: 'Test Movie',
        overview: 'A long enough overview for testing',
        vote_average: 7.5,
        vote_count: 1000
      };

      const result = movieResultSchema.parse(inputData);

      expect(result.image).toBe(fallbackImage);
    });

    it('catches short overview and uses default', () => {
      const inputData = {
        id: 1,
        backdrop_path: null,
        poster_path: null,
        release_date: '2023-05-15',
        title: 'Test Movie',
        overview: '\n',
        vote_average: 7.5,
        vote_count: 1000
      };

      const result = movieResultSchema.parse(inputData);

      expect(result.overview).toBe('No overview available');
    });

    it('throws an error when data type is invalid', () => {
      const inputData = {
        id: 2,
        backdrop_path: undefined,
        poster_path: null,
        release_date: '2023-05-15',
        title: null,
        overview: 'A long enough overview for testing',
        vote_average: 7.5,
        vote_count: 1000
      };

      expect(() => movieResultSchema.parse(inputData)).toThrow();
    });
  });

  describe('movieResultsListSchema', () => {
    it('transforms movie list data correctly', () => {
      const inputData = {
        page: 1,
        results: [{
          id: 1,
          backdrop_path: null,
          poster_path: null,
          release_date: '2023-05-15',
          title: 'Test Movie',
          overview: 'A long enough overview for testing',
          vote_average: 7.5,
          vote_count: 1000
        }],
        total_pages: 5,
        total_results: 50
      };

      const result = movieResultsListSchema.parse(inputData);

      expect(result).toEqual({
        page: 1,
        results: [{
          id: 1,
          year: '2023',
          title: 'Test Movie',
          overview: 'A long enough overview for testing',
          voteAverage: '7.5',
          voteCount: 1000,
          image: fallbackImage
        }],
        totalPages: 5,
        totalResults: 50
      });
    });

    it('throws an error when data type is invalid', () => {
      const inputData = {
        results: [{
          id: 1,
          backdrop_path: null,
          poster_path: null,
          release_date: '2023-05-15',
          title: 'Test Movie',
          overview: 'A long enough overview for testing',
          vote_average: 7.5,
          vote_count: 1000
        }],
      };

      expect(() => movieResultsListSchema.parse(inputData)).toThrow();
    });
  });

  describe('movieSchema', () => {
    it('transforms movie schema data correctly', () => {
      const inputData = {
        id: 1,
        title: 'Test Movie',
        overview: 'A detailed overview of the movie.',
        genres: [
          { id: 1, name: 'Action' },
          { id: 2, name: 'Adventure' }
        ],
        poster_path: '/poster.jpg',
        vote_average: 8.034,
        vote_count: 2000,
        origin_country: ['US', 'UK'],
        release_date: '2023-05-15'
      };

      const result = movieSchema.parse(inputData);

      expect(result).toEqual({
        id: 1,
        title: 'Test Movie',
        overview: 'A detailed overview of the movie.',
        originCountries: ['US', 'UK'],
        voteAverage: '8.0',
        voteCount: 2000,
        year: '2023',
        genres: [
          { id: 1, name: 'Action' },
          { id: 2, name: 'Adventure' }
        ],
        image: 'https://image.tmdb.org/t/p/w400/poster.jpg'
      });
    });

    it('uses fallback image when poster_path is null', () => {
      const inputData = {
        id: 1,
        title: 'Test Movie',
        overview: 'A detailed overview of the movie',
        genres: [
          { id: 1, name: 'Action' }
        ],
        poster_path: null,
        vote_average: 8.0,
        vote_count: 2000,
        origin_country: ['US'],
        release_date: '2023-05-15'
      };

      const result = movieSchema.parse(inputData);

      expect(result.image).toBe(fallbackImage);
    });

    it('catches short overview and uses default', () => {
      const inputData = {
        id: 1,
        title: 'Test Movie',
        overview: 'Short',
        genres: [
          { id: 1, name: 'Action' }
        ],
        poster_path: '/poster.jpg',
        vote_average: 8.0,
        vote_count: 2000,
        origin_country: ['US'],
        release_date: '2023-05-15'
      };

      const result = movieSchema.parse(inputData);

      expect(result.overview).toBe('No overview available');
    });

    it('throws an error when data type is invalid', () => {
      const inputData = {
        id: 1,
        title: 'Test Movie',
        overview: 'Short',
        poster_path: '/poster.jpg',
        vote_average: 8.0,
        vote_count: 2000,
        origin_country: ['US'],
        release_date: '2023-05-15'
      };

      expect(() => movieSchema.parse(inputData)).toThrow();
    });
  });
});