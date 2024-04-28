import { useEffect } from 'react';
import { useInView } from 'react-intersection-observer';
import {
  InfiniteData,
  InfiniteQueryObserverResult,
} from '@tanstack/react-query';
import { MovieSearchResult } from '@/modules/movies/domain/MovieSearchResult';

const useInfiniteScroll = (
  hasNextPage: boolean,
  fetchNextPage: () => Promise<
    InfiniteQueryObserverResult<InfiniteData<MovieSearchResult, unknown>, Error>
  >,
) => {
  const { ref, inView } = useInView();

  useEffect(() => {
    if (inView && hasNextPage) {
      fetchNextPage();
    }
  }, [inView, hasNextPage, fetchNextPage]);

  return { ref };
};

export default useInfiniteScroll;
