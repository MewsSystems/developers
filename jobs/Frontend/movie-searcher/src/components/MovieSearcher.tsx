import { MovieList } from './MovieList';
import { useMoviesInfinite } from '../hooks/useMovies';
import { useState } from 'react';
import { useDebounce } from '@uidotdev/usehooks';
import { FaSpinner } from 'react-icons/fa';

export const MovieSearcher = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  const { data, fetchNextPage, hasNextPage, isPending } = useMoviesInfinite(debouncedSearchTerm);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
  };

  return (
    <div className='flex flex-col items-center w-full gap-8'>
      <header>
        <input
          id='searchInput'
          name='searchInput'
          onChange={handleSearchChange}
          className='min-w-80'
          type='text'
          placeholder='Avengers, Batman, Superman, etc.'
        />
      </header>
      <div className='w-full flex flex-col gap-8 items-center'>
        {!debouncedSearchTerm && <h1>Popular movies</h1>}
        {isPending ? (
          <FaSpinner size='30px' className='animate-spin' />
        ) : (
          <MovieList movies={data?.pages.flatMap((page) => page.movies) ?? []} />
        )}
        {hasNextPage && (
          <div>
            <button onClick={() => fetchNextPage()}>Load more</button>
          </div>
        )}
      </div>
    </div>
  );
};
