import allMoviesSearch from "@/services/movies";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

//Adding movieName and page to queryKey will make the query run every time either of them change
export function useGetMoviesSearched(movieName: string, page: number) {
  return useQuery({
    queryKey: ["searchedMovies", movieName, page],
    queryFn: async () => await allMoviesSearch.getSearch(movieName, page),
    placeholderData: keepPreviousData,
  });
}
