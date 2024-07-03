import { useState, useCallback } from "react";
import { fetchMovies, fetchMovieDetail, Movie, MovieDetails } from "../api";

export const useMovies = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [movieDetail, setMovieDetail] = useState<MovieDetails | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const searchMovies = useCallback(async (query: string, page: number) => {
    setLoading(true);
    setError(null);
    try {
      const newMovies = await fetchMovies(query, page);
      setMovies((prevMovies) => [...prevMovies, ...newMovies]);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, []);

  const getMovieDetail = useCallback(async (id: string) => {
    setLoading(true);
    setError(null);
    try {
      const movie = await fetchMovieDetail(id);
      setMovieDetail(movie);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, []);

  const resetMovies = useCallback(() => {
    setMovies([]);
  }, []);

  return {
    movies,
    movieDetail,
    searchMovies,
    getMovieDetail,
    resetMovies,
    loading,
    error,
  };
};
