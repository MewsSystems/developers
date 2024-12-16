import { useState, useEffect } from "react";
import { Movie } from "../types";

interface UseMoviesFetchResult {
  data: Movie[];
  totalPages: number;
  isLoading: boolean;
  error: Error | null;
}

export const useFetchMovies = (
  baseURL: string,
  search: string,
  page: number
): UseMoviesFetchResult => {
  const [data, setData] = useState<Movie[]>([]);
  const [totalPages, setTotalPages] = useState<number>(0);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    const fetchMovies = async () => {
      if (!search.trim()) {
        setData([]);
        setTotalPages(0);
        return;
      }

      setIsLoading(true);
      setError(null);

      try {
        const response = await fetch(
          `${baseURL}&query=${encodeURIComponent(search)}&page=${page}`
        );
        if (!response.ok) {
          throw new Error("Failed to fetch movies");
        }

        const result = await response.json();
        setData(result.results || []);
        setTotalPages(result.total_pages || 0);
      } catch (err: any) {
        setError(err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchMovies();
  }, [baseURL, search, page]);

  return { data, totalPages, isLoading, error };
};
