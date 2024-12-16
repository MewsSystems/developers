import { MovieList } from './MovieList';
import { useMovies } from '../hooks/useMovies';
import { useState } from 'react';
import { useDebounce } from '@uidotdev/usehooks';

export const MovieSearcher = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 700);

  const { data: movies } = useMovies(debouncedSearchTerm);

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
      <div className='w-full'>
        <MovieList movies={movies || []} />
      </div>
    </div>
  );
};
