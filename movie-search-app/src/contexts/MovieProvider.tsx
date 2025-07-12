import { type ReactNode, useState } from "react";
import { getPopularMovies } from "../api/getPopularMovies";
import { MovieContext } from "./MovieContext";
import { getMovieDetails } from "../api/getMovieDetails";
import { searchMovieList } from "../api/searchMovies";
import type { Movie } from "../types/types";

// Provider
export const MovieProvider = ({ children }: { children: ReactNode }) => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [movieDetails, setMovieDetails] = useState<Movie>({} as Movie);
  const [searchQuery, setSearchQuery] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1); // Current page number, default to 1 if not provided
  const [totalPages, setTotalPages] = useState(1); // Number of items per page
  const [debouncedQuery, setDebouncedQuery] = useState(searchQuery);

  const itemsPerPage = 20; // Number of items per page

  const searchMovies = async () => {
    setLoading(true);
    setError(null);

    try {
      const response = await getPopularMovies(currentPage);

      if (response && response.results) {
        setMovies(response.results);
        setTotalPages(response.total_pages);
      } else {
        setError("No results found");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Failed to fetch movies");
      }
    } finally {
      setLoading(false);
    }
  };

  const getMovieInfo = async (movieId: number) => {
    setLoading(true);
    setError(null);

    try {
      const response = await getMovieDetails(movieId);

      if (response) {
        setMovieDetails(response);
      } else {
        setError("No details found for this movie");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Failed to fetch movie details");
      }
    } finally {
      setLoading(false);
    }
  };

  const searchMoviesByQuery = async (query: string, current_page: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await searchMovieList(query, current_page);
      if (response && response.results && response.total_pages !== undefined) {
        setMovies(response.results);
        setTotalPages(response.total_pages);
      } else {
        setError("No details found for this movie");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Failed to fetch movie details");
      }
    } finally {
      setLoading(false);
    }
  };

  // return the context provider with the state and functions
  return (
    <MovieContext
      value={{
        movies,
        loading,
        setLoading,
        error,
        searchMovies,
        movieDetails,
        getMovieInfo,
        itemsPerPage,
        currentPage,
        searchQuery,
        setSearchQuery,
        setCurrentPage,
        totalPages,
        searchMoviesByQuery,
        debouncedQuery,
        setDebouncedQuery
      }}
    >
      {children}
    </MovieContext>
  );
};
