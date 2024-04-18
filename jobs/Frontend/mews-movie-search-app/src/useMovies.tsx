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
};
export const useMovies = ({ page = 1 } = {} as useMoviesArgTypes) => {
  const [movies, setMovies] = React.useState<MovieListResponse | null>(null);

  const getList = async () => {
    console.log(page);
    const res = await fetch(
      `https://api.themoviedb.org/3/discover/movie?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }&include_adult=false&include_video=false&language=en-US&page=${page}&sort_by=popularity.desc`
    );
    const data = await res.json();
    setMovies(data);
  };

  React.useEffect(() => {
    getList();
  }, [page]);

  return { movies };
};
