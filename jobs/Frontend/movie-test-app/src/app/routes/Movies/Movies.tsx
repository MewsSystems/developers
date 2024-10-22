import { Link, useSearchParams } from 'react-router-dom';
import { useInfiniteMovies } from '../../api/movies.ts';
import { useEffect, useState } from 'react';
import { useElementVisible } from '../../../hooks/useElementVisible.ts';

export const Movies = () => {
  const [searchParams] = useSearchParams();
  const [userQuery, setUserQuery] = useState<string>(searchParams.get('query') || '');

  const { visible: isLastElementVisible, setRef } = useElementVisible();

  const moviesQuery = useInfiniteMovies({ searchParam: userQuery || '' });
  const movies = moviesQuery.data?.pages.flatMap((page) => page.data.results) || [];
  //there are some duplicate movies in the response, so we need to filter them out
  const uniqueMovies = Array.from(new Set(movies.map((movie) => movie.id))).map((id) =>
    movies.find((movie) => movie.id === id),
  );

  useEffect(() => {
    if (isLastElementVisible && !moviesQuery.isFetchingNextPage && moviesQuery.hasNextPage) {
      moviesQuery.fetchNextPage();
    }
  }, [isLastElementVisible, moviesQuery]);

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUserQuery(e.target.value);
  };

  return (
    <>
      <input onChange={handleSearchInput}></input>
      {uniqueMovies.map((movie, index) => (
        <div
          key={movie?.id.toString()}
          ref={(instance) => (index === uniqueMovies.length - 1 ? setRef(instance) : null)}
        >
          <Link to={`/movies/${movie?.id}`}>{movie?.title}</Link>
        </div>
      ))}
      {moviesQuery.isLoading && <>Loading...</>}
    </>
  );
};
