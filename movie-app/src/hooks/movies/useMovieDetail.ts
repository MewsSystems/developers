import { fetchAPI, movieDbApiKey } from "../../api/config.ts";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

export const useMovieDetail = (movieId: number) => {
  const getMovieDetail = fetchAPI(
    `movie/${movieId}/similar?api_key=${movieDbApiKey}`,
  );

  return useQuery({
    queryKey: ["movie", movieId],
    queryFn: () => getMovieDetail,
    placeholderData: keepPreviousData,
    enabled: !!movieId,
  });
};
