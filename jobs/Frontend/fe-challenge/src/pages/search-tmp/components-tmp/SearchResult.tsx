import React, { useMemo } from 'react';
import MovieCard from '@/pages/search-tmp/components-tmp/MovieCard';
import useSearchMovie from '@/pages/search-tmp/hooks-tmp/useSearchMovie';
import EmptySearch from '@/pages/search-tmp/components-tmp/EmptySearch';
import useInfiniteScroll from '@/pages/search-tmp/hooks-tmp/useInfiniteScroll';
import ResultsNumber from '@/pages/search-tmp/components-tmp/ResultsNumber';
import usePrefetchMovieDetail from '@/pages/movie-detail/hooks/usePrefetchMovieDetail';

interface SearchResultProps {
  query: string;
}

const SearchResult = ({ query }: SearchResultProps) => {
  const { data, hasNextPage, fetchNextPage } = useSearchMovie(query);
  const { ref } = useInfiniteScroll(hasNextPage, fetchNextPage);
  const noData = useMemo(() => data?.pages[0].totalResults === 0, [data]);
  const numResults = useMemo(() => data?.pages[0].totalResults, [data]);
  const { prefetch } = usePrefetchMovieDetail();

  return noData ? (
    <EmptySearch>
      <EmptySearch.Title>Nothing found 😔</EmptySearch.Title>
      <EmptySearch.Description>
        Use the search bar for another search
      </EmptySearch.Description>
    </EmptySearch>
  ) : (
    <>
      {numResults > 0 ? (
        <ResultsNumber count={numResults} query={query} />
      ) : null}
      <ul className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-5 gap-6">
        {data?.pages.map((group, i) => (
          <React.Fragment key={i}>
            {group.results.map((movie, index, results) => (
              <li
                key={movie.id}
                ref={index + 1 === results.length ? ref : undefined}
                onMouseEnter={() => prefetch(movie.id)}
              >
                <MovieCard movie={movie} />
              </li>
            ))}
          </React.Fragment>
        ))}
      </ul>
    </>
  );
};

export default SearchResult;
