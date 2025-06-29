import { useQuery, type UseQueryResult } from "@tanstack/react-query";
import type { MovieDetailsResponse } from "../api/types";
import { getMovieDetails } from "../api/requests";

export const useMovieDetails = (
  movieId: number
): UseQueryResult<MovieDetailsResponse, Error> => {
  return useQuery({
    queryKey: ["movieDetails", movieId],
    queryFn: () => getMovieDetails(movieId),
    enabled: movieId > 0,
  });
};
