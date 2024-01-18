import { useSelector } from '@/store';
import { currentSearchTerm } from '@/store/slices/Search/selectors';
import usePaginatedMovieSearch from '@/hooks/usePaginatedMovieSearch';
import Flex from '@/components/Flex';
import Pagination from '@/components/Pagination/Pagination';
import MovieListItem from '@/components/MovieListItem/MovieListItem';

const MovieList = () => {
  const currentSearchState = useSelector(currentSearchTerm);
  const moviesWithPagination = usePaginatedMovieSearch(currentSearchState);

  return (
    <>
      <Flex flexWrap="wrap" flexDirection="row" gridGap="16px">
        {
          moviesWithPagination.loading
            ? <div>loading...</div>
            : moviesWithPagination.error
              ? <div>error</div>
              : moviesWithPagination.movies?.map((movie) => (
            <MovieListItem {...movie} key={movie.id}/>
          ))
        }
      </Flex>
      <Flex justifyContent="center">
        <Pagination
          totalPages={moviesWithPagination.totalPages}
          currentPage={moviesWithPagination.page}
          onSetPage={moviesWithPagination.setPage}
          onNextPage={moviesWithPagination.increment}
          onPrevPage={moviesWithPagination.decrement}
        />
      </Flex>
    </>
  )
}

export default MovieList
