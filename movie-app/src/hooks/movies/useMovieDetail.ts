import { fetchAPI, movieDbApiKey } from "../../api/config.ts";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { useParams } from "react-router-dom";
import { Movie } from "./types.ts";

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
