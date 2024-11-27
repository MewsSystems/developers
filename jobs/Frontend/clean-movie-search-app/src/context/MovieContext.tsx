import React, { createContext, useContext, useState, useCallback } from 'react';
import { Movie, MovieSearchResponse } from '../api';
import { searchMovies } from '../api/movieApi';

// Context type definition with all the movie list related info and methods
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

// MovieProvider to provide the context to its children
export const MovieProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  // State variables within the context to manage movies, query, pagination, loading states, and errors accross the app
  const [movies, setMovies] = useState<Movie[]>([]);
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);
  const [paginationLoading, setPaginationLoading] = useState(false);
  const [error, setError] = useState('');
  const [lastSearchQuery, setLastSearchQuery] = useState('');

  // Function to handle searching movies based on a query and page number
  const handleSearch = useCallback(
    async (searchQuery: string, pageNum: number = 1) => {
      if (!searchQuery.trim()) {
        setMovies([]);
        return;
      }

      // Check cache for the same query when "returning" from movie details
      // If the search query is the same as the last search query, we are at the first page, and
      // there are already movies in the state, and the loading state is false, then return direclty.
      // This is done to prevent unnecessary API calls when the user just navigates back to the search results 
      // from a movie detail view.
      if (
        pageNum === 1 &&
        searchQuery === lastSearchQuery &&
        movies.length > 0 &&
        !loading
      ) {
        return;
      }

      try {
        // Set loading state based on whether it's a new search or a "next page search"
        if (pageNum === 1) {
          setLoading(true);
        } else {
          setPaginationLoading(true);
        }
        setError('');

        // Fetch the movies through the nicely improved and prodected API fetch :)
        const response: MovieSearchResponse = await searchMovies(
          searchQuery,
          pageNum
        );

        // Update state with the fetched movies or expand results if we are expanding to a new page
        if (pageNum === 1) {
          setMovies(response.results);
        } else {
          setMovies((prev) => [...prev, ...response.results]);
        }

        setTotalPages(response.total_pages);
        // We set the page which will be 1 in case of a new search
        setPage(pageNum);
        // This will be used for "cache" checking on return navigation
        setLastSearchQuery(searchQuery);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : 'An error occurred retrieving movies'
        );
        // If the failure is at a new search research movies, 
        // If its loading a new page, keep the currently loaded movies
        if (pageNum === 1) setMovies([]);
      } finally {
        setLoading(false);
        setPaginationLoading(false);
      }
    },
    [lastSearchQuery, movies.length, loading]
  );

  const loadMore = useCallback(async () => {
    // Load more movies only if we are not already loading and there are more pages to load
    // I want to prevent handling the (page < totalPages) logic only in the view
    if (page < totalPages) {
      await handleSearch(query, page + 1);
    }
  }, [query, page, totalPages, handleSearch]);

  // Could be used to allow the user to reset the search when clicking the title for example
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
