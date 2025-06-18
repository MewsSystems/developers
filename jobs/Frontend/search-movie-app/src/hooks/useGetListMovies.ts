import { useInfiniteQuery } from '@tanstack/react-query';
import { fetchListMovies } from '../api/fetch';

const useGetListMovies = ({ query }: { query: string }) => {
  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading } = useInfiniteQuery({
    queryKey: ['listMoviess', query],
    queryFn: ({ pageParam = 1 }) => fetchListMovies({ query, page: pageParam }),
    getNextPageParam: lastPage => {
      if (lastPage.page < lastPage.total_pages) {
        return lastPage.page + 1;
      }
      return undefined;
    },
    initialPageParam: 1,
  });
  return { data, fetchNextPage, hasNextPage, isFetchingNextPage, isLoading };
};

export { useGetListMovies };
