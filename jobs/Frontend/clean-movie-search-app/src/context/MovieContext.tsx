import React, { createContext, useContext, useState, useCallback } from 'react';
import { Movie, MovieSearchResponse } from '../api/types';
import { searchMovies } from '../api/movieApi';

interface MovieContextType {
  movies: Movie[];
  query: string;
  page: number;
  totalPages: number;
  loading: boolean;
  paginationLoading: boolean;
  error: string;
  setQuery: (query: string) => void;
  handleSearch: (
    searchQuery: string,
    pageNum?: number,
    forceReload?: boolean
  ) => Promise<void>;
  loadMore: () => Promise<void>;
  resetSearch: () => void;
}

const MovieContext = createContext<MovieContextType | undefined>(undefined);

export const MovieProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);
  const [paginationLoading, setPaginationLoading] = useState(false);
  const [error, setError] = useState('');
  const [lastSearchQuery, setLastSearchQuery] = useState('');

  const handleSearch = useCallback(
    async (searchQuery: string, pageNum: number = 1) => {
      if (!searchQuery.trim()) {
        setMovies([]);
        return;
      }

      // Only check cache for the same query when returning from movie details
      if (
        pageNum === 1 &&
        searchQuery === lastSearchQuery &&
        movies.length > 0 &&
        !loading
      ) {
        // Add loading check to prevent race conditions
        return;
      }

      try {
        if (pageNum === 1) {
          setLoading(true);
        } else {
          setPaginationLoading(true);
        }
        setError('');

        const response: MovieSearchResponse = await searchMovies(
          searchQuery,
          pageNum
        );

        if (pageNum === 1) {
          setMovies(response.results);
        } else {
          setMovies((prev) => [...prev, ...response.results]);
        }

        setTotalPages(response.total_pages);
        setPage(pageNum);
        setLastSearchQuery(searchQuery);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : 'An error occurred retrieving movies'
        );
        if (pageNum === 1) setMovies([]);
      } finally {
        setLoading(false);
        setPaginationLoading(false);
      }
    },
    [lastSearchQuery, movies.length, loading]
  );

  const loadMore = useCallback(async () => {
    if (page < totalPages) {
      await handleSearch(query, page + 1);
    }
  }, [query, page, totalPages, handleSearch]);

  const resetSearch = useCallback(() => {
    setMovies([]);
    setQuery('');
    setPage(1);
    setTotalPages(0);
    setError('');
    setLastSearchQuery('');
  }, []);

  return (
    <MovieContext.Provider
      value={{
        movies,
        query,
        page,
        totalPages,
        loading,
        paginationLoading,
        error,
        setQuery,
        handleSearch,
        loadMore,
        resetSearch,
      }}
    >
      {children}
    </MovieContext.Provider>
  );
};

export const useMovieContext = () => {
  const context = useContext(MovieContext);
  if (context === undefined) {
    throw new Error('useMovieContext must be used within a MovieProvider');
  }
  return context;
};
