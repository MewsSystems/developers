import { MovieListResponse } from "../types/movies";
import { domainURL } from "../utils/constant";
import { useInfiniteQuery } from "@tanstack/react-query";
import { customFetch } from "../utils/customFetch";

export const useMovies = (searchTerm: string) => {
  const getMovies = async ({ pageParam }: { pageParam: number }) => {
    const fetchURL = `${domainURL}movie/popular?api_key=${
      import.meta.env.VITE_TMDB_KEY
    }&include_adult=false&include_video=false&language=en-US&page=${pageParam}&sort_by=popularity.desc`;
    return await customFetch<MovieListResponse>(fetchURL);
  };
  const getMoviesBySearch = async ({
    pageParam,
    searchParam,
  }: {
    pageParam: number;
    searchParam: string;
  }) => {
    return await customFetch<MovieListResponse>(
      `${domainURL}search/movie?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }&query=${searchParam}&include_adult=false&include_video=false&language=en-US&page=${pageParam}&sort_by=popularity.desc`
    );
  };

  const {
    data,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isLoading,
    isFetchingNextPage,
    status,
  } = useInfiniteQuery({
    queryKey: ["movies", { searchTerm }],
    queryFn: ({ pageParam }) =>
      searchTerm
        ? getMoviesBySearch({
            pageParam,
            searchParam: searchTerm,
          })
        : getMovies({ pageParam }),
    initialPageParam: 1,
    getNextPageParam: (lastPage) => {
      if (lastPage.page < lastPage.total_pages) {
        return lastPage.page + 1;
      }
    },
  });

  return {
    data,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isLoading,
    isFetchingNextPage,
    status,
  };
};
