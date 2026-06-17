import { useParams } from "react-router-dom";

import { keepPreviousData, useQuery } from "@tanstack/react-query";

import { Movie } from "./types.ts";

import { fetchAPI, movieDbApiKey } from "@/api/config.ts";

export const useMovieDetail = () => {
  const { id } = useParams();

  const { data, isError, isLoading } = useQuery({
    queryKey: ["movie", id],
    queryFn: () => fetchAPI(`movie/${id}?api_key=${movieDbApiKey}`),
    placeholderData: keepPreviousData,
    enabled: !!id,
  });

  return {
    data: data as Movie,
    isError,
    isLoading,
  };
};
