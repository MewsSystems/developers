import { useInfiniteQuery } from '@tanstack/react-query';
import { fetchListMovies } from '../api/fetch';
import { useInputSearchMovie } from '../store/inputSearchMovieStore';
import { useDebounce } from './useDebounce';

const useGetListMovies = () => {
  const inputSearchMovie = useInputSearchMovie(state => state.inputSearchMovie);
  const inputSearchDebounced = useDebounce(inputSearchMovie, 300);

  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading, error } =
    useInfiniteQuery({
      queryKey: ['listMovies', inputSearchDebounced],
      queryFn: ({ pageParam = 1 }) => {
        return fetchListMovies({ query: inputSearchDebounced, page: pageParam });
      },
      getNextPageParam: lastPage => {
        if (lastPage.page < lastPage.total_pages) {
          return lastPage.page + 1;
        }
        return undefined;
      },
      enabled: !!inputSearchDebounced,
      initialPageParam: 1,
    });
  return {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isLoading,
    inputSearchDebounced,
    error,
  };
};

export { useGetListMovies };
