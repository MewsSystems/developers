import { fetchAPI, movieDbApiKey } from "../../api/config.ts";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

export const useSearchMovieList = (query: string, page: number = 1) => {
  const getMoviesList = fetchAPI(
    `search/movie?api_key=${movieDbApiKey}&query=${query}&page=${page}`,
  );

  return useQuery({
    queryKey: ["searchMovie", query, page],
    queryFn: () => getMoviesList,
    placeholderData: keepPreviousData,
    enabled: query.length > 1,
  });
};
