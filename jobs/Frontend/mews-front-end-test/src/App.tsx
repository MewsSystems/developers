import React from 'react';
import './App.css';
import {
  StyledAppContainer,
  StyledHeader,
  StyledMain,
  StyledSearchBox,
  StyledTitle,
} from './App.styled';
import { MovieCard } from './components/MovieCard';

function App() {
  return (
    <StyledAppContainer>
      <StyledHeader>
        <StyledTitle>Search for movies</StyledTitle>
      </StyledHeader>

      <StyledMain>
        <StyledSearchBox />
        <MovieCard />
        <MovieCard />
        <MovieCard />
      </StyledMain>
    </StyledAppContainer>
  );
}

export default App;
