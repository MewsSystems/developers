import { MovieList } from './MovieList';
import { useMoviesInfinite } from '../hooks/useMovies';
import { useState } from 'react';
import { useDebounce } from '@uidotdev/usehooks';

export const MovieSearcher = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 700);

  const { data, fetchNextPage, hasNextPage } = useMoviesInfinite(debouncedSearchTerm);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
  };

  return (
    <div className='flex flex-col items-center w-full gap-8'>
      <header>
        <form>
          <input
            name='searchInput'
            onChange={handleSearchChange}
            className='min-w-80'
            type='text'
            placeholder='Avengers, Batman, Superman, etc.'
          />
        </form>
      </header>
      <div className='w-full flex flex-col gap-8'>
        <MovieList movies={data?.pages.flatMap((page) => page.movies) ?? []} />
        {hasNextPage && (
          <div>
            <button onClick={() => fetchNextPage()}>Load more</button>
          </div>
        )}
      </div>
    </div>
  );
};
