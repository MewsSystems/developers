import { FC } from 'react';
import styled from 'styled-components';
import { useDebounce } from '../../../hooks/useDebounce';
import { useMoviesSearch } from '../hooks';
import { SearchBar, MovieCard } from '../components';
import { useSearch } from '../context';
import { Movie } from '../types';

const Wrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 2rem;
`;

const ResultsContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
`;

const MovieList = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
  margin-top: 2rem;
  width: 100%;
`;

const LoadMoreButton = styled.button`
  margin: 2rem auto;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  cursor: pointer;
`;

const Movies: FC = () => {
  const { query, setQuery } = useSearch();
  const debouncedQuery = useDebounce(query);

  const { data, hasNextPage, fetchNextPage, isFetchingNextPage, isLoading, isError } =
    useMoviesSearch(debouncedQuery);

  const handleLoadMore = () => {
    fetchNextPage();
  };
  return (
    <Wrapper>
      <SearchBar query={query} setQuery={setQuery} />
      {debouncedQuery && (
        <ResultsContainer>
          {isLoading && <div>Loading...</div>}
          {isError && <div>Error fetching movies.</div>}
          {data && (
            <MovieList>
              {data?.pages.map((page) =>
                page.results.map((movie: Movie) => <MovieCard key={movie.id} movie={movie} />)
              )}
            </MovieList>
          )}
          {hasNextPage && (
            <LoadMoreButton onClick={handleLoadMore} disabled={isFetchingNextPage}>
              {isFetchingNextPage ? 'Loading more...' : 'Load More'}
            </LoadMoreButton>
          )}
        </ResultsContainer>
      )}
    </Wrapper>
  );
};

export default Movies;
