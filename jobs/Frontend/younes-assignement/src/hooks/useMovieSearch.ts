import { useState, useEffect, useCallback } from "react";
import { fetchMovies } from "../api/tmdb";
import type { MovieType } from "../types/movie";

type UseMovieSearchResult = {
  query: string;
  setQuery: (q: string) => void;
  movies: MovieType[];
  loading: boolean;
  error: string | null;
  page: number;
  search: (term: string, nextPage?: number) => void;
  reset: () => void;
};

const useMovieSearch = (): UseMovieSearchResult => {
  const [query, setQuery] = useState<string>(() => {
    return sessionStorage.getItem("searchQuery") || "";
  });

  const [movies, setMovies] = useState<MovieType[]>(() => {
    const saved = sessionStorage.getItem("searchMovies");
    return saved ? JSON.parse(saved) : [];
  });

  const [page, setPage] = useState<number>(() => {
    const saved = sessionStorage.getItem("searchPage");
    return saved ? Number(saved) : 1;
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Persist state to sessionStorage
  useEffect(() => {
    sessionStorage.setItem("searchQuery", query);
    sessionStorage.setItem("searchMovies", JSON.stringify(movies));
    sessionStorage.setItem("searchPage", page.toString());
  }, [query, movies, page]);

  const search = useCallback(async (term: string, nextPage?: number) => {
    if (!term) return;

    const pageNum = nextPage ?? 1;
    setLoading(true);
    setError(null);
    try {
      const data = await fetchMovies(term, pageNum);
      setMovies((prev) =>
        pageNum === 1 ? data.results : [...prev, ...data.results],
      );
      setPage(pageNum);
    } catch (err) {
      setError("Something went wrong. Please try again.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  const reset = useCallback(() => {
    setQuery("");
    setMovies([]);
    setPage(1);
  }, []);

  return { query, setQuery, movies, loading, error, page, search, reset };
};

export default useMovieSearch;
