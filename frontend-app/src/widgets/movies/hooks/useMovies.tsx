import { useCallback, useEffect, useState } from 'react';
import type { Movie } from '@/entities/movie';
import { getPopularMovies, searchMovies } from '@/entities/movie/api';

interface UseMoviesState {
  movies: Array<Movie>;
  isLoading: boolean;
  error: string | null;
  currentPage: number;
  totalPages: number;
  searchQuery: string;
}

interface UseMoviesReturn extends UseMoviesState {
  handleSearch: (query: string, abortController?: AbortController) => void;
  handlePageChange: (page: number) => void;
  setMovies: (movies: Array<Movie>) => void;
  setIsLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  setTotalPages: (pages: number) => void;
}

/**
 * Hook for managing movies state, search and pagination
 */
export const useMovies = (): UseMoviesReturn => {
  const [state, setState] = useState<UseMoviesState>({
    movies: [],
    isLoading: false,
    error: null,
    currentPage: 1,
    totalPages: 1,
    searchQuery: '',
  });

  /**
   * Movie search handler
   */
  const handleSearch = useCallback(async (query: string, abortController?: AbortController) => {
    setState(prev => ({
      ...prev,
      searchQuery: query,
      currentPage: 1, // Reset to first page on new search
      isLoading: true,
      error: null,
    }));

    try {
      if (query.trim()) {
        // Search movies by query
        const response = await searchMovies(query, 1, abortController?.signal);
        
        if (!abortController?.signal.aborted) {
          setState(prev => ({
            ...prev,
            isLoading: false,
            movies: response.results,
            totalPages: Math.min(response.total_pages, 500),
          }));
        }
      } else {
        // If query is empty, load popular movies
        const response = await getPopularMovies(1);
        
        if (!abortController?.signal.aborted) {
          setState(prev => ({
            ...prev,
            isLoading: false,
            movies: response.results,
            totalPages: Math.min(response.total_pages, 500), 
          }));
        }
      }
    } catch (error) {
      if (error instanceof Error && error.name === 'AbortError') {
        setState(prev => ({
          ...prev,
          isLoading: false,
          error: 'Request aborted',
        }));
      } else if (!abortController?.signal.aborted) {
        setState(prev => ({
          ...prev,
          isLoading: false,
          error: error instanceof Error ? error.message : 'An error occurred while searching movies',
        }));
      }
    }
  }, []);

  /**
   * Page change handler
   */
  const handlePageChange = useCallback(async (page: number) => {
    setState(prev => ({
      ...prev,
      currentPage: page,
      isLoading: true,
      error: null,
    }));

    try {
      let response;
      
      // Get current search query from state
      const currentSearchQuery = state.searchQuery;
      
      if (currentSearchQuery.trim()) {
        // If there's a search query, search by it
        response = await searchMovies(currentSearchQuery, page);
      } else {
        // Otherwise load popular movies
        response = await getPopularMovies(page);
      }
      
      setState(prev => ({
        ...prev,
        isLoading: false,
        movies: response.results,
        totalPages: Math.min(response.total_pages, 500), // Ограничиваем максимальное количество страниц до 500
      }));
    } catch (error) {
      setState(prev => ({
        ...prev,
        isLoading: false,
        error: error instanceof Error ? error.message : 'An error occurred while loading movies',
      }));
    }
  }, [state.searchQuery]);

  /**
   * Setters for state updates
   */
  const setMovies = useCallback((movies: Array<Movie>) => {
    setState(prev => ({ ...prev, movies }));
  }, []);

  const setIsLoading = useCallback((isLoading: boolean) => {
    setState(prev => ({ ...prev, isLoading }));
  }, []);

  const setError = useCallback((error: string | null) => {
    setState(prev => ({ ...prev, error }));
  }, []);

  const setTotalPages = useCallback((totalPages: number) => {
    setState(prev => ({ ...prev, totalPages }));
  }, []);

  /**
   * Load popular movies on initialization
   */
  useEffect(() => {
    const loadInitialMovies = async () => {
      setState(prev => ({ ...prev, isLoading: true, error: null }));
      
      try {
        const response = await getPopularMovies(1);
        setState(prev => ({
          ...prev,
          isLoading: false,
          movies: response.results,
          totalPages: Math.min(response.total_pages, 500), // Ограничиваем максимальное количество страниц до 500
        }));
      } catch (error) {
        setState(prev => ({
          ...prev,
          isLoading: false,
          error: error instanceof Error ? error.message : 'An error occurred while loading movies',
        }));
      }
    };

    loadInitialMovies();
  }, []);

  return {
    ...state,
    handleSearch,
    handlePageChange,
    setMovies,
    setIsLoading,
    setError,
    setTotalPages,
  };
};