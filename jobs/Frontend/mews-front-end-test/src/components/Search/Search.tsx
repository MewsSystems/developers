import React, { Children } from 'react';
import { MovieCard } from '../MovieCard/MovieCard';
import { StyledMain } from './Search.styled';
import { useMovies } from '../../hook/useMovies';
import { SearchBox } from '../SearchBox/SearchBox';
import { SearchControls } from '../SearchControls/SearchControls';

const Search = () => {
  const {
    movies,
    searchQuery,
    setSearchQuery,
    page,
    numberOfPages,
    incrementPageNumber,
    decrementPageNumber,
  } = useMovies();

  return (
    <StyledMain>
      <SearchBox searchQuery={searchQuery} setSearchQuery={setSearchQuery} />

      <SearchControls
        page={page}
        numberOfPages={numberOfPages}
        incrementPageNumber={incrementPageNumber}
        decrementPageNumber={decrementPageNumber}
        showControls={Boolean(searchQuery)}
      />

      {Children.toArray(movies.map((movie) => <MovieCard movie={movie} />))}

      <SearchControls
        page={page}
        numberOfPages={numberOfPages}
        incrementPageNumber={incrementPageNumber}
        decrementPageNumber={decrementPageNumber}
        showControls={Boolean(searchQuery)}
      />
    </StyledMain>
  );
};

export { Search };
