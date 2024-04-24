import React, { Children } from 'react';
import { MovieCard } from '../MovieCard/MovieCard';
import { StyledMain } from './Search.styled';
import { useMovies } from '../../hook/useMovies';
import { SearchBox } from '../SearchBox/SearchBox';

const Search = () => {
  const {
    movies,
    searchQuery,
    setSearchQuery,
    page,
    incrementPageNumber,
    decrementPageNumber,
  } = useMovies();

  return (
    <StyledMain>
      <SearchBox searchQuery={searchQuery} setSearchQuery={setSearchQuery} />
      {Children.toArray(movies.map((movie) => <MovieCard movie={movie} />))}

      <div>
        <button onClick={decrementPageNumber}>Back</button>
        {page}
        <button onClick={incrementPageNumber}>Next</button>
      </div>
    </StyledMain>
  );
};

export { Search };
