import React, { Children, FC } from 'react';
import { MovieCard } from '../MovieCard/MovieCard';
import { StyledListItem, StyledMain } from './Search.styled';
import { UseMovies, useMovies } from '../../hook/useMovies';
import { SearchBox } from '../SearchBox/SearchBox';
import { SearchControls } from '../SearchControls/SearchControls';
import { Link } from 'react-router-dom';
import {
  setCurrentMovie,
  setCurrentSearch,
} from '../../redux/movies/movieSlice';

interface SearchDisplayProps {
  useMoviesBundle: UseMovies;
}

const SearchDisplay: FC<SearchDisplayProps> = ({ useMoviesBundle }) => {
  const {
    movies,
    searchQuery,
    setSearchQuery,
    page,
    numberOfPages,
    incrementPageNumber,
    decrementPageNumber,
    dispatch,
  } = useMoviesBundle;

  return (
    <StyledMain>
      <SearchBox searchQuery={searchQuery} setSearchQuery={setSearchQuery} />

      <SearchControls
        page={page}
        numberOfPages={numberOfPages}
        incrementPageNumber={incrementPageNumber}
        decrementPageNumber={decrementPageNumber}
        showControls={Boolean(searchQuery)}
        id={'top'}
      />

      <ul>
        {Children.toArray(
          movies.map((movie) => {
            return (
              <StyledListItem>
                <Link
                  to={'/details'}
                  onClick={() => {
                    dispatch(setCurrentMovie(movie));
                    dispatch(
                      setCurrentSearch({
                        movies,
                        searchQuery,
                        page,
                        numberOfPages,
                      }),
                    );
                  }}
                >
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
        id={'bottom'}
      />
    </StyledMain>
  );
};

const Search = () => <SearchDisplay useMoviesBundle={useMovies()} />;

export { Search, SearchDisplay };
