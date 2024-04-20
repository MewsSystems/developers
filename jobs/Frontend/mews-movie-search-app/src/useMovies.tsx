import React from "react";
import { MovieListResponse } from "./types/movies";
import { domainURL } from "./utils/constant";
import { useSpinDelay } from "spin-delay";

type useMoviesArgTypes = {
  page?: number;
  searchTerm?: string;
};
const emptyMovieListResponse: MovieListResponse = {
  results: [],
  page: 0,
  total_pages: 0,
  total_results: 0,
};
export const useMovies = (
  { page = 1, searchTerm = "" } = {} as useMoviesArgTypes
) => {
  const [movies, setMovies] = React.useState<MovieListResponse | null>(null);
  const [isLoading, setLoading] = React.useState(false);
  const showIsLoading = useSpinDelay(isLoading, {
    delay: 500,
    minDuration: 200,
  });
  const querySearch = searchTerm ? `query=${searchTerm}` : null;

  const getList = async () => {
    try {
      setLoading(true);
      const fetchURL = `${domainURL}movie/popular?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }&include_adult=false&include_video=false&language=en-US&page=${page}&sort_by=popularity.desc`;
      const res = await fetch(fetchURL);
      const data = await res.json();

      setMovies(data);
      setLoading(false);
    } catch (error) {
      setMovies(emptyMovieListResponse);
      setLoading(false);
    }
  };

  const getMoviesBySearch = async () => {
    try {
      setLoading(true);
      const res = await fetch(
        `${domainURL}search/movie?api_key=${
          import.meta.env.VITE_TMDB_KEY
        }&${querySearch}&include_adult=false&include_video=false&language=en-US&page=${page}&sort_by=popularity.desc`
      );
      const data = await res.json();
      setMovies(data);
      setLoading(false);
    } catch (error) {
      setMovies(emptyMovieListResponse);
      setLoading(false);
    }
  };

  React.useEffect(() => {
    if (searchTerm !== "") {
      getMoviesBySearch();
    } else {
      getList();
    }
  }, [page, searchTerm]);

  return { movies, isLoading: showIsLoading };
};
