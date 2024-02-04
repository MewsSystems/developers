import { useEffect, useState, useCallback } from "react";
import { apiKey } from "../config/secrets";
import { Movie } from "../types/types";

const useGetMovieList = (page: number) => {
  const [movieList, setMovieList] = useState<Movie[]>([]);
  const [maxPage, setMaxPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [query, setQuery] = useState("");

  const fetchMovies = useCallback(async () => {
    setLoading(true);
    try {
      const response = await fetch(
        `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${query}&page=${page}`
      );
      const data = await response.json();
      setMovieList((prevData) => [
        ...prevData,
        ...data.results,
      ]);
      setMaxPage(data.total_pages);
    } catch (error: any) {
      setError(error);
    }
    setLoading(false);
  }, [query, page]);

  useEffect(() => {
    setMovieList([]);
    if (query !== "") {
      fetchMovies();
    }
  }, [query]);

  useEffect(() => {
    if (query !== "") {
      fetchMovies();
    }
  }, [page]);

  return { movieList, maxPage, loading, error, setQuery };
};

export default useGetMovieList;
