import { useSearchParams } from 'react-router-dom';
import { useInfiniteMovies } from '../../api/movies.ts';
import { useState } from 'react';

export const Movies = () => {
  const [searchParams] = useSearchParams();
  const [userQuery, setUserQuery] = useState<string>(searchParams.get('query') || '');

  const moviesQuery = useInfiniteMovies({ searchParam: userQuery || '' });
  const movies = moviesQuery.data?.pages.flatMap((page) => page.data.results) || [];

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUserQuery(e.target.value);
  };

  return (
    <>
      <input onChange={handleSearchInput}></input>
      {movies.map((movie) => (
        <div key={movie.id}>{movie.title}</div>
      ))}
      {moviesQuery.isLoading && <>Loading...</>}
    </>
  );
};
