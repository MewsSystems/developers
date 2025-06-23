import { useParams } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";

import { getMovieDetail } from "../api";
import { calculateDuration } from "../utils/utils";

export function useMovieDetailsQuery() {
  const { movieId } = useParams();
  const {
    isLoading,
    data: movie,
    isError,
    error,
  } = useQuery({
    queryKey: ["movie-detail", movieId],
    queryFn: () => getMovieDetail(movieId),
  });
  const movieRuntime = useMemo(
    () => calculateDuration(movie?.runtime),
    [movie]
  );

  return {
    movie,
    isLoading,
    isError,
    error,
    movieRuntime,
  };
}
