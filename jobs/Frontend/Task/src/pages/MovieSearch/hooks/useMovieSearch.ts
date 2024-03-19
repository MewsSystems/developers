import { useState } from "react";
import { useQuerySearchMovie } from "../../../hooks/api/useQuerySearchMovie/useQuerySearchMovie";

export const useMovieSearch = () => {
  const [ query, setQuery ] = useState("");
  const [ page, setPage ] = useState(1);

  const { data, isLoading, isError } = useQuerySearchMovie({ 
    query,
    page,
  });

  const movieList = data?.results?.map(movie => ({
    id: movie.id,
    title: movie.title,
    overview: movie.overview,
    releaseDate: movie.release_date,
    originalLanguage: movie.original_language,
    path: movie.poster_path,
  }));

  return {
    movieList: movieList || [],
    page: data?.page,
    pagesTotal: data?.total_pages,
    isLoading,
    isError,
    query,
    setPage,
    setQuery
  };
};
