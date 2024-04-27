import React, { useMemo } from 'react';
import MovieCard from '@/pages/Search/components/MovieCard';
import useSearchMovie from '@/pages/Search/hooks/useSearchMovie';
import EmptySearch from '@/pages/Search/components/EmptySearch';
import useInfiniteScroll from '@/pages/Search/hooks/useInfiniteScroll';
import ResultsNumber from '@/pages/Search/components/ResultsNumber';

interface SearchResultProps {
  query: string;
}

const SearchResult = ({ query }: SearchResultProps) => {
  const { data, hasNextPage, fetchNextPage } = useSearchMovie(query);
  const { ref } = useInfiniteScroll(hasNextPage, fetchNextPage);
  const noData = useMemo(() => data?.pages[0].totalResults === 0, [data]);
  const numResults = useMemo(() => data?.pages[0].totalResults, [data]);

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
