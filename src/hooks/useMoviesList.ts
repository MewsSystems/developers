import {useQuery, useQueryClient} from '@tanstack/react-query';
import type {MovieSearchResponse} from '../api/movieApi/types';
import type {ApiErrorResponseDetails} from '../api/movieApi/utils/types';
import {fetchMoviesList} from '../api/movieApi/endpoints/fetchMoviesList';
import {MAX_USER_INPUT_SEARCH_LENGTH} from '../pages/MoviesListPage/components/SearchInput/SearchInput';

const FIVE_MINUTES = 5 * 60 * 1000;

export const useMoviesList = (searchQuery: string, page: number) => {
  const queryClient = useQueryClient();

  const {data, error, isLoading, isFetching} = useQuery<
    MovieSearchResponse,
    ApiErrorResponseDetails
  >({
    queryKey: ['movies', searchQuery, page],
    queryFn: () => fetchMoviesList(searchQuery, page),
    enabled: searchQuery.length > 0 && searchQuery.length <= MAX_USER_INPUT_SEARCH_LENGTH,
    staleTime: FIVE_MINUTES,
    gcTime: FIVE_MINUTES * 2,
    placeholderData: (previousItems) => (searchQuery ? previousItems : undefined),
  });

  const clearMoviesCache = () =>
    queryClient.removeQueries({
      queryKey: ['movies', searchQuery, page],
      exact: true,
    });

  return {
    movies: data?.results || [],
    totalPages: data?.total_pages || 0,
    error,
    isLoading,
    isFetching,
    clearMoviesCache,
  };
};
