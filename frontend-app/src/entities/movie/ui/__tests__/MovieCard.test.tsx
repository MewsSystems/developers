/**
 * Tests for MovieCard component
 */
import { render, screen } from '@testing-library/react';
import { QueryClientProvider } from '@tanstack/react-query';
import { describe, expect, it, vi } from 'vitest';

import { MovieCard } from '../MovieCard';
import type { Movie } from '../../types';
import { createTestQueryClient } from '@/shared/lib/test-utils';

// Mock TanStack Router
vi.mock('@tanstack/react-router', () => ({
  Link: ({ children, to, params, className }: any) => (
    <a href={`${to.replace('$movieId', params.movieId)}`} className={className}>
      {children}
    </a>
  ),
}));

// Test wrapper component
const TestWrapper = ({ children }: { children: React.ReactNode }) => {
  const queryClient = createTestQueryClient();
  
  return (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );
};

// Mock movie data
const mockMovie: Movie = {
  id: 1,
  title: 'Test Movie',
  overview: 'This is a test movie overview.',
  poster_path: '/test-poster.jpg',
  backdrop_path: '/test-backdrop.jpg',
  release_date: '2023-01-01',
  vote_average: 8.5,
  vote_count: 1000,
  genre_ids: [28, 12],
  adult: false,
  original_language: 'en',
  original_title: 'Test Movie',
  popularity: 100.5,
  video: false,
};

describe('MovieCard Component', () => {
  describe('Basic Rendering', () => {
    it('should render movie card with all basic information', () => {
      // Act
      render(
        <TestWrapper>
          <MovieCard movie={mockMovie} />
        </TestWrapper>
      );

      // Assert
      expect(screen.getByText('Test Movie')).toBeDefined();
      expect(screen.getByText('2023')).toBeDefined();
      expect(screen.getByText('8.5')).toBeDefined();
    });

    it('should render movie poster with correct src and alt', () => {
      // Act
      render(
        <TestWrapper>
          <MovieCard movie={mockMovie} />
        </TestWrapper>
      );

      // Assert
      const posterImage = screen.getByAltText('Test Movie');
      expect(posterImage).toBeDefined();
      expect(posterImage.getAttribute('src')).toBe('https://image.tmdb.org/t/p/w500/test-poster.jpg');
    });

    it('should render link to movie details page', () => {
      // Act
      render(
        <TestWrapper>
          <MovieCard movie={mockMovie} />
        </TestWrapper>
      );

      // Assert
      const link = screen.getByRole('link');
      expect(link).toBeDefined();
      expect(link.getAttribute('href')).toBe('/movie/1');
    });
  });

  describe('Poster Image Handling', () => {
    it('should use placeholder when poster_path is null', () => {
      // Arrange
      const movieWithoutPoster: Movie = {
        ...mockMovie,
        poster_path: null as any,
      };

      // Act
      render(
        <TestWrapper>
          <MovieCard movie={movieWithoutPoster} />
        </TestWrapper>
      );

      // Assert
      const posterImage = screen.getByAltText('Test Movie');
      expect(posterImage.getAttribute('src')).toBe('/placeholder-poster.png');
    });
  });

  describe('Release Date Formatting', () => {
    it('should display release year when release_date is provided', () => {
      // Act
      render(
        <TestWrapper>
          <MovieCard movie={mockMovie} />
        </TestWrapper>
      );

      // Assert
      expect(screen.getByText('2023')).toBeDefined();
    });
  });

  describe('Rating Display', () => {
    it('should display formatted rating when vote_average is provided', () => {
      // Act
      render(
        <TestWrapper>
          <MovieCard movie={mockMovie} />
        </TestWrapper>
      );

      // Assert
      expect(screen.getByText('8.5')).toBeDefined();
    });
  });
});