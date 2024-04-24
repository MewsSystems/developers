import React, { FC } from 'react';
import { MovieCard } from '../MovieCard/MovieCard';
import { StyledMain, StyledSearchBox } from './Search.styled';

const Search: FC = () => {
  return (
    <StyledMain>
      <StyledSearchBox />
      <MovieCard />
      <MovieCard />
      <MovieCard />
    </StyledMain>
  );
};

export { Search };
