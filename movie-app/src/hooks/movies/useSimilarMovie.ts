import { fetchAPI, movieDbApiKey } from "../../api/config.ts";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

export const useSimilarMovie = (movieId: number) => {
  const getSimilarMovies = fetchAPI(
    `movie/${movieId}/similar?api_key=${movieDbApiKey}`,
  );

  return useQuery({
    queryKey: ["similarMovie", movieId],
    queryFn: () => getSimilarMovies,
    placeholderData: keepPreviousData,
    enabled: !!movieId,
  });
};
