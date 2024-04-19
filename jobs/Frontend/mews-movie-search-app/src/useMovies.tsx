import React from "react";

export type MovieListResponse = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

export type Movie = {
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};
type useMoviesArgTypes = {
  page?: number;
  searchTerm?: string;
};
const domainURL = `https://api.themoviedb.org/3/`;
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
  const querySearch = searchTerm ? `query=${searchTerm}` : null;

  const getList = async () => {
    try {
      const fetchURL = `${domainURL}movie/popular?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }&include_adult=false&include_video=false&language=en-US&page=${page}&sort_by=popularity.desc`;
      const res = await fetch(fetchURL);
      const data = await res.json();

      setMovies(data);
    } catch (error) {
      setMovies(emptyMovieListResponse);
    }
  };

  const getMoviesBySearch = async () => {
    try {
      const res = await fetch(
        `${domainURL}search/movie?api_key=${
          import.meta.env.VITE_TMDB_KEY
        }&${querySearch}&include_adult=false&include_video=false&language=en-US&page=${page}&sort_by=popularity.desc`
      );
      const data = await res.json();
      setMovies(data);
    } catch (error) {
      setMovies(emptyMovieListResponse);
    }
  };

  React.useEffect(() => {
    if (searchTerm) {
      getMoviesBySearch();
    } else {
      getList();
    }
  }, [page, searchTerm]);

  return { movies };
};
