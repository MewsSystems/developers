import { useParams } from "react-router-dom";

import { keepPreviousData, useQuery } from "@tanstack/react-query";

import { MoviesSearchApiResponse } from "./types.ts";

import { fetchAPI, movieDbApiKey } from "@/api/config.ts";

export const useSimilarMovies = () => {
  const { id } = useParams();

  const getSimilarMovies = fetchAPI(
    `movie/${id}/similar?api_key=${movieDbApiKey}`,
  );

  const { data, isLoading, isError } = useQuery({
    queryKey: ["similarMovie", id],
    queryFn: () => getSimilarMovies,
    placeholderData: keepPreviousData,
    enabled: !!id,
  });

  return {
    data: (data as MoviesSearchApiResponse)?.results,
    isError,
    isLoading,
  };
};
