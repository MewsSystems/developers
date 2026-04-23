import React from 'react';
import { StyledAppContainer } from './Home.styled';
import { Search } from '../components/Search/Search';
import { Header } from '../components/Header/Header';

function Home() {
  return (
    <StyledAppContainer>
      <Header />
      <Search />
    </StyledAppContainer>
  );
}

export { Home };
