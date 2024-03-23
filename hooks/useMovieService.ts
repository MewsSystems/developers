import { getMovieDetails, getMovieList } from "@/services/movieService";
import { useInfiniteQuery, useQuery, } from "@tanstack/react-query";

export const useGetMovieList = (searchTerm: string) => {
  return useInfiniteQuery({
    queryKey: ['getMovieList', searchTerm],
    queryFn: ({ pageParam = 1 }) => getMovieList(searchTerm, pageParam),
    initialPageParam: 1,
    getNextPageParam: (lastPage, allPages) => {
      const nextPage = lastPage.results.length ? allPages.length + 1 : undefined;
      return nextPage;
    },
  })
}

export const useGetMovieDetails = (movieId: string) => {
  return useQuery({
    queryKey: ['movie'],
    queryFn: async () => {
      const response = await getMovieDetails(movieId);
      return response;
    }
  })
};