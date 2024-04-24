import React, { Children } from 'react';
import { MovieCard } from '../MovieCard/MovieCard';
import { StyledListItem, StyledMain } from './Search.styled';
import { useMovies } from '../../hook/useMovies';
import { SearchBox } from '../SearchBox/SearchBox';
import { SearchControls } from '../SearchControls/SearchControls';
import { Link } from 'react-router-dom';

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

      <ul>
        {Children.toArray(
          movies.map((movie) => {
            return (
              <StyledListItem>
                <Link to={'/details'}>
                  <MovieCard movie={movie} />
                </Link>
              </StyledListItem>
            );
          }),
        )}
      </ul>

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
